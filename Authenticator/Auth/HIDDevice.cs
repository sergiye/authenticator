using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Authenticator
{
	/// <summary>
	/// Class that loads all current USB devices so we can find the YubiKey
	/// Credit to http://brandonw.net/
	/// </summary>
	public class HidDevice : IDisposable
	{
		#region P/Invoke

		[Flags]
		private enum EFileAttributes : uint
		{
			Readonly = 0x00000001,
			Hidden = 0x00000002,
			System = 0x00000004,
			Directory = 0x00000010,
			Archive = 0x00000020,
			Device = 0x00000040,
			Normal = 0x00000080,
			Temporary = 0x00000100,
			SparseFile = 0x00000200,
			ReparsePoint = 0x00000400,
			Compressed = 0x00000800,
			Offline = 0x00001000,
			NotContentIndexed = 0x00002000,
			Encrypted = 0x00004000,
			WRITE_THROUGH = 0x80000000,
			Overlapped = 0x40000000,
			NoBuffering = 0x20000000,
			RandomAccess = 0x10000000,
			SequentialScan = 0x08000000,
			DeleteOnClose = 0x04000000,
			BackupSemantics = 0x02000000,
			PosixSemantics = 0x01000000,
			OpenReparsePoint = 0x00200000,
			OpenNoRecall = 0x00100000,
			FirstPipeInstance = 0x00080000
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct HiddAttributes
		{
			public Int32 Size;
			public Int16 VendorID;
			public Int16 ProductID;
			public Int16 VersionNumber;
		}

		//[StructLayout(LayoutKind.Sequential)]
		//private struct GUID
		//{
		//	public int Data1;
		//	public System.UInt16 Data2;
		//	public System.UInt16 Data3;
		//	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		//	public byte[] data4;
		//}

		[DllImport("hid.dll", SetLastError = true)]
		private static extern void HidD_GetHidGuid(
			ref Guid lpHidGuid);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetupDiGetClassDevs(
			ref Guid classGuid,
			IntPtr enumerator, //[MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
			IntPtr hwndParent,
			UInt32 flags
			);

		[StructLayout(LayoutKind.Sequential)]
		private struct SpDeviceInterfaceData
		{
			public Int32 cbSize;
			public Guid InterfaceClassGuid;
			public Int32 Flags;
			public UIntPtr Reserved;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct SpDeviceInterfaceDetailData
		{
			public UInt32 cbSize;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string DevicePath;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		private struct PspDeviceInterfaceDetailData
		{
			public Int32 cbSize;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string DevicePath;
		}

		[DllImport(@"setupapi.dll", SetLastError = true)]
		private static extern Boolean SetupDiGetDeviceInterfaceDetail(
			IntPtr hDevInfo,
			ref SpDeviceInterfaceData deviceInterfaceData,
			IntPtr deviceInterfaceDetailData,
			UInt32 deviceInterfaceDetailDataSize,
			out UInt32 requiredSize,
			IntPtr deviceInfoData
		);

		[DllImport(@"setupapi.dll", SetLastError = true)]
		private static extern Boolean SetupDiGetDeviceInterfaceDetail(
			IntPtr hDevInfo,
			ref SpDeviceInterfaceData deviceInterfaceData,
			ref SpDeviceInterfaceDetailData deviceInterfaceDetailData,
			UInt32 deviceInterfaceDetailDataSize,
			out UInt32 requiredSize,
			IntPtr deviceInfoData
		);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern Boolean SetupDiEnumDeviceInterfaces(
			IntPtr hDevInfo,
			//ref SP_DEVINFO_DATA devInfo,
			IntPtr devInvo,
			ref Guid interfaceClassGuid,
			Int32 memberIndex,
			ref SpDeviceInterfaceData deviceInterfaceData
		);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern UInt16 SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern SafeFileHandle CreateFile(
			string lpFileName,
			UInt32 dwDesiredAccess,
			UInt32 dwShareMode,
			IntPtr lpSecurityAttributes,
			UInt32 dwCreationDisposition,
			UInt32 dwFlagsAndAttributes,
			IntPtr hTemplateFile
		);

		[DllImport("kernel32.dll")]
		private static extern int CloseHandle(SafeFileHandle hObject);
		[DllImport("kernel32.dll")]
		private static extern int CloseHandle(IntPtr hObject);

		[DllImport("hid.dll", SetLastError = true)]
		private static extern int HidD_GetPreparsedData(
			SafeFileHandle hObject,
			ref IntPtr pPhidpPreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		private static extern int HidP_GetCaps(
			IntPtr pPhidpPreparsedData,
			ref HidpCaps myPhidpCaps);

		[DllImport("hid.dll")]
		internal extern static bool HidD_SetOutputReport(
			IntPtr hidDeviceObject,
			byte[] lpReportBuffer,
			uint reportBufferLength);

		[DllImport("hid.dll")]
		private static extern Boolean HidD_GetAttributes(IntPtr hidDeviceObject, ref HiddAttributes attributes);

		[DllImport("kernel32.dll")]
		private static extern int WriteFile(SafeFileHandle hFile, ref byte lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, int lpOverlapped);

		[StructLayout(LayoutKind.Sequential)]
		private struct HidpCaps
		{
			public UInt16 Usage;
			public UInt16 UsagePage;
			public UInt16 InputReportByteLength;
			public UInt16 OutputReportByteLength;
			public UInt16 FeatureReportByteLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
			public UInt16[] Reserved;
			public UInt16 NumberLinkCollectionNodes;
			public UInt16 NumberInputButtonCaps;
			public UInt16 NumberInputValueCaps;
			public UInt16 NumberInputDataIndices;
			public UInt16 NumberOutputButtonCaps;
			public UInt16 NumberOutputValueCaps;
			public UInt16 NumberOutputDataIndices;
			public UInt16 NumberFeatureButtonCaps;
			public UInt16 NumberFeatureValueCaps;
			public UInt16 NumberFeatureDataIndices;
		}

		private const int DIGCF_PRESENT = 0x00000002;
		private const int DIGCF_DEVICEINTERFACE = 0x00000010;
		private const int DIGCF_INTERFACEDEVICE = 0x00000010;
		private const uint GENERIC_READ = 0x80000000;
		private const uint GENERIC_WRITE = 0x40000000;
		private const uint FILE_SHARE_READ = 0x00000001;
		private const uint FILE_SHARE_WRITE = 0x00000002;
		private const int OPEN_EXISTING = 3;

		#endregion

		#region Declarations

		private bool found = false;
		private Guid guid;
		private IntPtr hDeviceInfo = IntPtr.Zero;
		private SpDeviceInterfaceData spDeviceInterfaceData;
		private string devicePath;
		private SafeFileHandle hidHandle;
		private FileStream stream;

		#endregion

		#region Public Events

		public event EventHandler<HidDeviceDataReceivedEventArgs> DataReceived;

		#endregion

		#region Constructors / Teardown

		public HidDevice(string devicePath)
		{
			_Init(devicePath, false);
		}

		public HidDevice(int vendorId, int productId)
		{
			_Init(vendorId, productId, false);
		}

		public HidDevice(int vendorId, int productId, bool throwNotFoundError)
		{
			_Init(vendorId, productId, throwNotFoundError);
		}

		public HidDevice(string devicePath, bool throwNotFoundError)
		{
			_Init(devicePath, throwNotFoundError);
		}

		#endregion

		#region Public Properties

		public int InputReportLength { get; private set; }
		public int OutputReportLength { get; private set; }

		public bool Found
		{
			get
			{
				return found;
			}
		}

		#endregion

		#region Public Methods

		public class HidDeviceEntry
		{
			public string Path { get; set; }
			public int VendorId { get; set; }
			public int ProductId { get; set; }

			public HidDeviceEntry(string path, int vid, int pid)
			{
				Path = path;
				VendorId = vid;
				ProductId = pid;
			}
		}

		public static List<HidDeviceEntry> GetAllDevices(int? vendorId = null, int? productId = null)
		{
			return GetAllDevices(new Guid(Guid.Empty.ToString()), vendorId, productId);
		}
		public static List<HidDeviceEntry> GetAllDevices(Guid guid, int? vendorId = null, int? productId = null)
		{
			var index = 0;
			//GUID guid = new GUID();
			var devices = new List<HidDeviceEntry>();

			if (guid == Guid.Empty)
			{
				HidD_GetHidGuid(ref guid);
			}
			var devicesHandle = SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero, DIGCF_INTERFACEDEVICE | DIGCF_PRESENT);
			var diData = new SpDeviceInterfaceData();
			diData.cbSize = Marshal.SizeOf(diData);

			while (SetupDiEnumDeviceInterfaces(devicesHandle, IntPtr.Zero, ref guid, index, ref diData))
			{
				//Get the buffer size
				SetupDiGetDeviceInterfaceDetail(devicesHandle, ref diData, IntPtr.Zero, 0, out var size, IntPtr.Zero);

				// Uh...yeah.
				var diDetail = new SpDeviceInterfaceDetailData();
				diDetail.cbSize = (uint)(IntPtr.Size == 8 ? 8 : 5);

				//Get detailed information
				if (SetupDiGetDeviceInterfaceDetail(devicesHandle, ref diData, ref diDetail, size, out size, IntPtr.Zero))
				{
					//Get a handle to this device
					var handle = CreateFile(diDetail.DevicePath, 4 /*GENERIC_WRITE*/, 3 /*FILE_SHARE_READ | FILE_SHARE_WRITE*/, IntPtr.Zero, 4 /*OPEN_EXISTING*/, 0, IntPtr.Zero);
					if (handle.IsInvalid == false)
					{
						//Get this device's attributes
						var attrib = new HiddAttributes();
						attrib.Size = Marshal.SizeOf(attrib);
						if (HidD_GetAttributes(handle.DangerousGetHandle(), ref attrib))
						{
							var vid = attrib.VendorID & 0xFFFF;
							var pid = attrib.ProductID & 0xFFFF;
							//See if this is one we care about
							if ((!vendorId.HasValue || vid == vendorId.Value) &&
								(!productId.HasValue || pid == productId.Value))
							{
								devices.Add(new HidDeviceEntry(diDetail.DevicePath, vid, pid));
								break;
							}
						}
					}

					//Close the handle
					handle.Close();
					//CloseHandle(handle);
				}

				//Move on
				index++;
			}

			SetupDiDestroyDeviceInfoList(devicesHandle);

			return devices;
		}

		public static bool IsDetected(int vendorId, int productId)
		{
			var device = new HidDevice(vendorId, productId, false);
			var ret = device.Found;

			device.Dispose();

			return ret;
		}

		public void Write(byte reportType, byte[] data)
		{
			var bytesSent = 0;

			do
			{
				var buffer = new byte[OutputReportLength];
				buffer[0] = reportType;
				for (var i = 1; i < buffer.Length; i++)
				{
					if (bytesSent < data.Length)
					{
						buffer[i] = data[bytesSent];
						bytesSent++;
					}
					else
					{
						buffer[i] = 0;
					}
				}

				HidD_SetOutputReport(hidHandle.DangerousGetHandle(), buffer, (uint)buffer.Length);
			} while (bytesSent < data.Length);
		}

		public void Disconnect()
		{
			try
			{
				stream.Close();
			}
			catch
			{
			}

			try
			{
				CloseHandle(hidHandle);
			}
			catch
			{
			}

			SetupDiDestroyDeviceInfoList(hDeviceInfo);
		}

		public void Dispose()
		{
			try
			{
				Disconnect();
			}
			catch
			{
				//Don't care...
			}
		}

		#endregion

		#region Private Methods

		private void _Init(int vendorId, int productId, bool throwNotFoundError)
		{
			var devices = GetAllDevices(vendorId, productId);

			if (devices != null && devices.Count > 0)
			{
				_Init(devices[0].Path, throwNotFoundError);
			}
			else
			{
				if (throwNotFoundError)
					throw new InvalidOperationException("Device not found");
			}
		}

		private void _Init(string devicePath, bool throwNotFoundError)
		{
			bool result;
			var deviceCount = 0;
			uint size;

			guid = new Guid();
			HidD_GetHidGuid(ref guid);

			hDeviceInfo = SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero, DIGCF_INTERFACEDEVICE | DIGCF_PRESENT);

			do
			{
				spDeviceInterfaceData = new SpDeviceInterfaceData();
				spDeviceInterfaceData.cbSize = Marshal.SizeOf(spDeviceInterfaceData);
				result = SetupDiEnumDeviceInterfaces(hDeviceInfo, IntPtr.Zero, ref guid, deviceCount, ref spDeviceInterfaceData);
				SetupDiGetDeviceInterfaceDetail(hDeviceInfo, ref spDeviceInterfaceData, IntPtr.Zero, 0, out var requiredSize, IntPtr.Zero);
				size = requiredSize;
				var diDetail = new SpDeviceInterfaceDetailData();
				diDetail.cbSize = (uint)(IntPtr.Size == 8 ? 8 : 5);
				SetupDiGetDeviceInterfaceDetail(hDeviceInfo, ref spDeviceInterfaceData, ref diDetail,
					size, out requiredSize, IntPtr.Zero);
				this.devicePath = diDetail.DevicePath;

				if (this.devicePath == devicePath)
				{
					found = true;
					spDeviceInterfaceData = new SpDeviceInterfaceData();
					spDeviceInterfaceData.cbSize = Marshal.SizeOf(spDeviceInterfaceData);
					SetupDiEnumDeviceInterfaces(hDeviceInfo, IntPtr.Zero, ref guid, deviceCount, ref spDeviceInterfaceData);
					size = 0;
					requiredSize = 0;
					SetupDiGetDeviceInterfaceDetail(hDeviceInfo, ref spDeviceInterfaceData, IntPtr.Zero, size, out requiredSize, IntPtr.Zero);
					SetupDiGetDeviceInterfaceDetail(hDeviceInfo, ref spDeviceInterfaceData, IntPtr.Zero, size, out requiredSize, IntPtr.Zero);
					hidHandle = CreateFile(this.devicePath, (uint)FileAccess.ReadWrite, (uint)FileShare.ReadWrite, IntPtr.Zero, (uint)FileMode.Open, (uint)EFileAttributes.Overlapped, IntPtr.Zero);

					//Get report lengths
					var preparsedDataPtr = (IntPtr)0xffffffff;
					if (HidD_GetPreparsedData(hidHandle, ref preparsedDataPtr) != 0)
					{
						var caps = new HidpCaps();
						HidP_GetCaps(preparsedDataPtr, ref caps);
						OutputReportLength = caps.OutputReportByteLength;
						InputReportLength = caps.InputReportByteLength;

						stream = new FileStream(hidHandle, FileAccess.ReadWrite, InputReportLength, true);
						var buffer = new byte[InputReportLength];
						stream.BeginRead(buffer, 0, buffer.Length, OnReadData, buffer);
					}

					break;
				}

				deviceCount++;
			} while (result);

			if (!found)
			{
				if (throwNotFoundError)
					throw new InvalidOperationException("Device not found");
			}
		}

		private void OnReadData(IAsyncResult result)
		{
			var buffer = (byte[])result.AsyncState;

			try
			{
				stream.EndRead(result);
				var receivedData = new byte[InputReportLength - 1];
				Array.Copy(buffer, 1, receivedData, 0, receivedData.Length);

				if (receivedData != null)
				{
					if (DataReceived != null)
					{
						DataReceived(this, new HidDeviceDataReceivedEventArgs(buffer[0], receivedData));
					}
				}

				var buf = new byte[buffer.Length];
				stream.BeginRead(buf, 0, buffer.Length, OnReadData, buf);
			}
			catch
			{
			}
		}

		#endregion
	}

  public class HidDeviceDataReceivedEventArgs : EventArgs
	{
    #region Declarations

    private byte reportType;
    private byte[] data;

    #endregion

    #region Constructors / Teardown

		public HidDeviceDataReceivedEventArgs(byte reportType, byte[] data)
    {
      this.reportType = reportType;
      this.data = data;
    }

    #endregion

    #region Public Properties

    public byte ReportType
    {
      get
      {
        return reportType;
      }
    }

    public byte[] Data
    {
      get
      {
        return data;
      }
    }

    #endregion
  }
}

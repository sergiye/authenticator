using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Authenticator {
  public class SingleGlobalInstance : IDisposable {
    public bool HasHandle { get; set; }

    private Mutex mutex;

    private void InitMutex() {
      HasHandle = false;

      var appGuid = ((GuidAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false)
        .GetValue(0)).Value;
      var userGuid = WindowsIdentity.GetCurrent().User.Value;
      var mutexId = string.Format("Global\\{{{0}}}-{{{1}}}", userGuid, appGuid);
      mutex = new Mutex(false, mutexId);

      var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
        MutexRights.FullControl, AccessControlType.Allow);
      var securitySettings = new MutexSecurity();
      securitySettings.AddAccessRule(allowEveryoneRule);
      mutex.SetAccessControl(securitySettings);
    }

    public SingleGlobalInstance(int timeOut = Timeout.Infinite) {
      InitMutex();
      try {
        HasHandle = mutex.WaitOne(timeOut, false);
        if (HasHandle == false) {
          throw new TimeoutException("Timeout waiting for exclusive access on SingleInstance");
        }
      }
      catch (AbandonedMutexException) {
        HasHandle = true;
      }
    }

    public void Dispose() {
      if (mutex != null) {
        if (HasHandle) {
          mutex.ReleaseMutex();
        }

        mutex.Dispose();
      }
    }
  }
}
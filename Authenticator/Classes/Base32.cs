using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Authenticator {
  public class Base32 {
    private static string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    private static readonly int[] numberTrailingZerosLookup = {
      32, 0, 1, 26, 2, 23, 27, 0, 3, 16, 24, 30, 28, 11, 0, 13, 4, 7, 17,
      0, 25, 22, 31, 15, 29, 10, 12, 6, 0, 21, 14, 9, 5, 20, 8, 19, 18
    };

    private static readonly Base32 instance = new Base32(defaultAlphabet);

    public static Base32 GetInstance(string alphabet = null) {
      return (alphabet == null ? instance : new Base32(alphabet));
    }

    private char[] digits;
    private int mask;
    private int shift;
    private Dictionary<char, int> map;

    protected Base32(string alphabet) {
      // initialise the decoder and precalculate the char map
      digits = alphabet.ToCharArray();
      mask = digits.Length - 1;
      shift = NumberOfTrailingZeros(digits.Length);
      map = new Dictionary<char, int>();
      for (var i = 0; i < digits.Length; i++) {
        map.Add(digits[i], i);
      }
    }

    private static int NumberOfTrailingZeros(int i) {
      return numberTrailingZerosLookup[(i & -i) % 37];
    }

    public byte[] Decode(string encoded) {
      // remove whitespace and any separators
      encoded = Regex.Replace(encoded, @"[\s-]+", "");

      // Google implementation ignores padding
      encoded = Regex.Replace(encoded, @"[=]*$", "");

      // convert as uppercase
      encoded = encoded.ToUpper(System.Globalization.CultureInfo.InvariantCulture);

      // handle zero case
      if (encoded.Length == 0) {
        return new byte[0];
      }

      var encodedLength = encoded.Length;
      var outLength = encodedLength * shift / 8;
      var result = new byte[outLength];
      var buffer = 0;
      var next = 0;
      var bitsLeft = 0;
      foreach (var c in encoded.ToCharArray()) {
        if (map.ContainsKey(c) == false) {
          throw new Base32DecodingException("Illegal character: " + c);
        }

        buffer <<= shift;
        buffer |= map[c] & mask;
        bitsLeft += shift;
        if (bitsLeft >= 8) {
          result[next++] = (byte) (buffer >> (bitsLeft - 8));
          bitsLeft -= 8;
        }
      }
      // We'll ignore leftover bits for now.
      //
      // if (next != outLength || bitsLeft >= SHIFT) {
      //  throw new DecodingException("Bits left: " + bitsLeft);
      // }

      return result;
    }

    public string Encode(byte[] data) {
      if (data.Length == 0) {
        return string.Empty;
      }

/*
			// _shift is the number of bits per output character, so the length of the
			// output is the length of the input multiplied by 8/_shift, rounded up.
			if (data.Length >= (1 << 28))
			{
				// The computation below will fail, so don't do it.
				throw new IllegalArgumentException();
			}

			// calculate result length
			int outputLength = (data.Length * 8 + _shift - 1) / _shift;
			StringBuilder result = new StringBuilder(outputLength);
*/

      var result = new StringBuilder();

      // encode data and map chars into result buffer
      int buffer = data[0];
      var next = 1;
      var bitsLeft = 8;
      while (bitsLeft > 0 || next < data.Length) {
        if (bitsLeft < shift) {
          if (next < data.Length) {
            buffer <<= 8;
            buffer |= (data[next++] & 0xff);
            bitsLeft += 8;
          }
          else {
            var pad = shift - bitsLeft;
            buffer <<= pad;
            bitsLeft += pad;
          }
        }

        var index = mask & (buffer >> (bitsLeft - shift));
        bitsLeft -= shift;
        result.Append(digits[index]);
      }

      return result.ToString();
    }
  }

  class Base32DecodingException : ApplicationException {
    public Base32DecodingException(string msg)
      : base(msg) {
    }
  }
}
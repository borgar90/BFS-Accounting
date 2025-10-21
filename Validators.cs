using System;
using System.Text.RegularExpressions;

namespace AccountingApp
{
 public static class Validators
 {
 public static string NormalizeOrgNumber(string org)
 {
 if (string.IsNullOrWhiteSpace(org)) return string.Empty;
 var digitsOnly = string.Empty;
 foreach (var c in org)
 {
 if (char.IsDigit(c)) digitsOnly += c;
 }
 return digitsOnly;
 }

 // Mod11 validation used earlier
 public static bool IsValidOrgNumber(string org)
 {
 if (string.IsNullOrWhiteSpace(org)) return true;
 var digitsOnly = NormalizeOrgNumber(org);
 if (digitsOnly.Length !=9) return false;
 int[] digits = new int[9];
 for (int i =0; i <9; i++) digits[i] = digitsOnly[i] - '0';
 int[] weights = new int[] {3,2,7,6,5,4,3,2 };
 int sum =0;
 for (int i =0; i <8; i++) sum += digits[i] * weights[i];
 int remainder =11 - (sum %11);
 int checkDigit;
 if (remainder ==11) checkDigit =0;
 else if (remainder ==10) return false;
 else checkDigit = remainder;
 return digits[8] == checkDigit;
 }

 public static bool IsValidEmail(string email)
 {
 if (string.IsNullOrWhiteSpace(email)) return true;
 // simple regex
 try
 {
 return Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.IgnoreCase);
 }
 catch
 {
 return false;
 }
 }

 public static bool IsValidPhone(string phone)
 {
 if (string.IsNullOrWhiteSpace(phone)) return true;
 // allow digits, spaces, + and -.
 var digitsOnly = string.Empty;
 foreach (var c in phone)
 {
 if (char.IsDigit(c)) digitsOnly += c;
 }
 // reasonable length check
 return digitsOnly.Length >=6 && digitsOnly.Length <=15;
 }
 }
}

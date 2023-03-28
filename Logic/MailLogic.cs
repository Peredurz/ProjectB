using System;

class MailCode
{
    static int GenerateCode()
    {
      Random RandomInt = new Random(); 
      return RandomInt.Next();
    }
}
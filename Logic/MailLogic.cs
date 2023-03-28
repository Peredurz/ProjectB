using System;

class MailLogic
{
    public int GenerateCode()
    {
        Random RandomInt = new Random(10); 
        return RandomInt.Next();
    }
}
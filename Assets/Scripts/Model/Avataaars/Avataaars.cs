using System;
using AvataaarsOptionEnums;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public class Avataaars
{
    public Avataaars()
    {
        
    }
    public List<string> GetEnums()
    {
        var nameSpace = "AvataaarsOptionEnums";
        Assembly asm = Assembly.GetExecutingAssembly();
        return asm.GetTypes()
            .Where(type => type.Namespace == nameSpace)
            .Select(type => type.Name).ToList();
    }

}

using System.Collections.Generic;

public interface IAliasEntity 
{

    public string Alias { get; set; }

    public AliasEntityTag Tag { get; set; }

}


public enum AliasEntityTag
{

    None,


}
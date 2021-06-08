using System.Collections.Generic;

public interface IQuest
{
    string GetShortDescription();
    Dictionary<EntityType, int> GetRequiredEntities();   
}
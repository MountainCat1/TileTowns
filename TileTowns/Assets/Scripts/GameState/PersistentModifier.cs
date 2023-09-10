public interface IPersistentModifier
{
    public int Housing { get; set; }
    public int WorkSlots { get; set; }
}

public class PersistentModifier : IPersistentModifier
{
    public virtual int Housing { get; set; }
    public virtual int WorkSlots { get; set; }
}
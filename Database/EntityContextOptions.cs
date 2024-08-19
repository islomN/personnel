namespace Database;

public record EntityContextOptions(string ConnectionString)
{
    public EntityContextOptions() : this((String)null!) { }
}
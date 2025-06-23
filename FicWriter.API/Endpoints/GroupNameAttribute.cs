namespace FicWriter.API.Endpoints;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class GroupNameAttribute : Attribute
{
    public string GroupName { get; }
    public string Tag { get; set; } = string.Empty;

    public GroupNameAttribute(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            throw new ArgumentException("Group name cannot be null or empty.", nameof(groupName));
        }

        GroupName = groupName;
    }
}

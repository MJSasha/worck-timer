namespace QuickActions.Api.Identity.IdentityCheck
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IdentityAttribute : Attribute
    {
        public string[] RoleNames { get; set; }

        public IdentityAttribute(params string[] roleNames)
        {
            RoleNames = roleNames;
        }
    }
}
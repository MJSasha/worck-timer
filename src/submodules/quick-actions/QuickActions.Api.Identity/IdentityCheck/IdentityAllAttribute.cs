namespace QuickActions.Api.Identity.IdentityCheck
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IdentityAllAttribute : Attribute
    {
        public string[] RoleNames { get; set; }

        public IdentityAllAttribute(params string[] roleNames)
        {
            RoleNames = roleNames;
        }
    }
}
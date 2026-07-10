using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPI.Models;

namespace MyFirstWebAPI.Filters
{
    public class CheckPermissionAttribute : TypeFilterAttribute
    {
        public CheckPermissionAttribute(Permission permission) : base(typeof(CheckPermissionFilter))
        {
            Arguments = new object[] { permission };

            /*
             * HOW THIS CUSTOM AUTHORIZATION PIPELINE WORKS
             * --------------------------------------------
             *  1. The Trigger (The Attribute):
             * When we decorate an endpoint with [CheckPermission(Permission.ReadProduct)], 
             * the specific enum (e.g., Permission.ReadProduct) is passed into this 
             * attribute's constructor (line 8).
             * 
             *  2. The Bridge (Arguments & typeof):
             * - The 'base(typeof(CheckPermissionFilter))' tells ASP.NET Core that 
             * this attribute is tied to the CheckPermissionFilter class.
             * - The 'Arguments = new object[] { permission };' line takes the 
             * permission we passed in and packs it into a built-in delivery box 
             * to send down to that filter.
             * 
             *  3. The Destination (The Filter Class):
             * Behind the scenes, ASP.NET Core instantiates the CheckPermissionFilter. 
             * It unpacks the 'Arguments' array and injects the permission directly 
             * into the filter's constructor parameter (requiredPermission). Finally, 
             * the filter saves it to a private readonly field (_requiredPermission) line 17 (CheckPermissionFilter.cs) 
             * so it can be used during the database query.
             */
        }
    }
}

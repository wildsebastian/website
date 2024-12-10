// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Areas.Admin.Pages.Account
{
    public static class ManageNavPages
    {
        private static string BlogIndex => "/Blog/Index";
        private static string BlogCreate => "/Blog/Create";

        private static string TagIndex => "/Tag/Index";
        private static string TagCreate => "/Tag/Create";

        private static string Profile => "/Account/Manage/Profile";
        private static string ExternalLogins => "/Account/Manage/ExternalLogins";
        private static string Logout => "/Account/Logout";


        public static string BlogIndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, BlogIndex);
        public static string BlogCreateNavClass(ViewContext viewContext) => PageNavClass(viewContext, BlogCreate);

        public static string TagIndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, TagIndex);
        public static string TagCreateNavClass(ViewContext viewContext) => PageNavClass(viewContext, TagCreate);

        public static string ProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);
        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);
        public static string LogoutNavClass(ViewContext viewContext) => PageNavClass(viewContext, Logout);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            string activePage = viewContext.ActionDescriptor.DisplayName;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase)
              ? "bg-indigo-700 text-white"
              : "text-indigo-200 hover:text-white hover:bg-indigo-700";
        }
    }
}
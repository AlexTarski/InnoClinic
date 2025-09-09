using System.ComponentModel;

namespace InnoClinic.Authorization.Business
{
    public enum ClientType
    {
        [Description("employee_ui")] EmployeeUI,
        [Description("client_ui")] ClientUI,
        [Description("profiles")] ProfilesAPI,
        [Description("offices")] OfficesAPI
    }
}
using CRM_Core.ViewModel;

namespace CRM_Core.Utility
{
    public interface ITextProvider
    {
        Task<ApiResponse<MessageResourceVewModel>> SendSMS(string toPhone, string smsText);
    }
}


public partial class UserCreditentials
{
    public string email { get; set; }
    public string username { get; set; }

    public UserCreditentials(string _Username, string _Email)
    {
        this.email = _Email;
        this.username = _Username;
    }
}
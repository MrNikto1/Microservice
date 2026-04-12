namespace Frontend;

public class UserState
{
    public int CurrentUserId { get; private set; } = 0;
    public string? CurrentUserName { get; private set; }
    public string? CurrentUserEmail { get; private set; }
    public bool IsAuthenticated => CurrentUserId > 0;

    public event Action? OnChange;

    public void SetCurrentUser(int id, string name, string email)
    {
        CurrentUserId = id;
        CurrentUserName = name;
        CurrentUserEmail = email;
        NotifyStateChanged();
    }

    public void Logout()
    {
        CurrentUserId = 0;
        CurrentUserName = null;
        CurrentUserEmail = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}

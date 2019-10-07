namespace Annium.Id.Db.Entities
{
    internal class User : BaseIdEntity
    {
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
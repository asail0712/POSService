using XPlan.Entities;

namespace Common.Entities
{
    public class StaffDataEntity : EntityBase
    {
        public string Account { get; set; }         // 帳號
        public string PasswordHash { get; set; }    // 密碼（記得存 Hash，不存明文）
        public string Name { get; set; }            // 名字
        public bool IsActive { get; set; }          // 帳號是否啟用
        public override string SearchKey
        {
            get => Account.ToString();
            set { /* 不做事 */}
        }
    }
}

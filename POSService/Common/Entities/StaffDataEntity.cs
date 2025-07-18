using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using XPlan.Entities;

namespace Common.Entities
{
    public class StaffDataEntity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }    // 建立時間
        public DateTime UpdatedAt { get; set; }    // 更新時間

        public string Account { get; set; }         // 帳號
        public string PasswordHash { get; set; }    // 密碼（記得存 Hash，不存明文）
        public string Name { get; set; }            // 名字
        public bool IsActive { get; set; }          // 帳號是否啟用
    }
}

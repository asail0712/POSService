using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace XPlan.Entities
{
    public abstract class EntityBase: IEntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }    // 建立時間
        public DateTime UpdatedAt { get; set; }    // 更新時間

        // 預設使用Id當作搜尋key
        public virtual string SearchKey
        {
            get => Id ?? string.Empty;
            set { /* 不做事 */}
        }
    }
}

using System;
using WebApp.Data.Entities;

namespace WebApp.Data.Services.Users
{
    public class ShareeModel
    {
        public ShareeModel()
        {
        }

        public ShareeModel(ListShareeMappingEntity entity) : this(entity?.User)
        {
        }

        public ShareeModel(UserEntity entity)
        {
            if(entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            Id = entity.Id;
            Name = entity.UserName;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
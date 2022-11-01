namespace DatingApp.Api.Entities
{
    public class UserLike
    {
        public AppUser LikerPerson {get;set;} //the person that likes someone
        public Guid LikerPersonId {get;set;}

        public AppUser LikedByPerson {get;set;} //the person that is liked by someone
        public Guid LikedByPersonId {get;set;}
    }
}
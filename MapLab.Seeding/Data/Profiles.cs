using MapLab.Data.Entities;

namespace MapLab.Seeding.Data
{
    public static class Profiles
    {
        public static List<(Profile, string?)> InitialProfiles => new()
        {
            (new Profile { UserName = "sophia.hughes", Email = "sophia.hughes@gmail.com", CreatedOn = new DateTime(2024, 12, 1), Bio = "Passionate about exploring the geography of Europe." }, "female-profile-picture-1.jpeg"),
            (new Profile { UserName = "liam.thompson", Email = "liam.t@hotmail.com", CreatedOn = new DateTime(2024, 12, 2), Bio = "Loves creating maps of historical landmarks." }, "male-profile-picture-1.jpeg"),
            (new Profile { UserName = "olivia.martin", Email = "olivia.martin@yahoo.com", CreatedOn = new DateTime(2024, 12, 3), Bio = "Interested in cartography and map design." }, "female-profile-picture-2.jpeg"),
            (new Profile { UserName = "noah.wilson", Email = "noah.wilson@gmail.com", CreatedOn = new DateTime(2024, 12, 4) }, "male-profile-picture-2.jpeg"),
            (new Profile { UserName = "ava.baker", Email = "ava.baker@aol.com", CreatedOn = new DateTime(2024, 12, 5), Bio = "Exploring the natural wonders of the world through maps." }, "female-profile-picture-3.jpeg"),
            (new Profile { UserName = "james.moore", Email = "james_moore@gmail.com", CreatedOn = new DateTime(2024, 12, 6), Bio = "Specializes in mapping urban areas." }, "male-profile-picture-3.jpeg"),
            (new Profile { UserName = "mia.taylor", Email = "miat@icloud.com", CreatedOn = new DateTime(2024, 12, 7) }, "female-profile-picture-4.jpeg"),
            (new Profile { UserName = "elijah.anderson", Email = "elijah.anderson@yahoo.com", CreatedOn = new DateTime(2024, 12, 8), Bio = "Enjoys creating topographic maps." }, "male-profile-picture-4.jpeg"),
            (new Profile { UserName = "amelia.jackson", Email = "amelia.jackson@gmail.com", CreatedOn = new DateTime(2024, 12, 9), Bio = "Fascinated by ancient maps and their history." }, "female-profile-picture-5.jpeg"),
            (new Profile { UserName = "lucas.white", Email = "lucas.white@hotmail.com", CreatedOn = new DateTime(2024, 12, 10) }, "male-profile-picture-5.jpeg"),
            (new Profile { UserName = "harper.hall", Email = "harper.hall@gmail.com", CreatedOn = new DateTime(2024, 12, 11), Bio = "Mapping the biodiversity of rainforests." }, null),
            (new Profile { UserName = "benjamin.lee", Email = "benji.lee@outlook.com", CreatedOn = new DateTime(2024, 12, 12), Bio = "Focuses on mapping transportation networks." }, "male-profile-picture-6.jpeg"),
            (new Profile { UserName = "evelyn.green", Email = "e.green@yahoo.com", CreatedOn = new DateTime(2024, 12, 13) }, "female-profile-picture-6.jpeg"),
            (new Profile { UserName = "henry.king", Email = "henry.king@gmail.com", CreatedOn = new DateTime(2024, 12, 14), Bio = "Exploring the geography of mountain ranges." }, "male-profile-picture-7.jpeg"),
            (new Profile { UserName = "abigail.wright", Email = "abigail.w@icloud.com", CreatedOn = new DateTime(2024, 12, 15), Bio = "Loves creating maps of hiking trails." }, "female-profile-picture-7.jpeg"),
            (new Profile { UserName = "jack.walker", Email = "jack_walker@live.com", CreatedOn = new DateTime(2024, 12, 16) }, null),
            (new Profile { UserName = "ella.scott", Email = "ellascott@gmail.com", CreatedOn = new DateTime(2024, 12, 17), Bio = "Passionate about mapping coastal regions." }, "female-profile-picture-8.jpeg"),
            (new Profile { UserName = "sebastian.young", Email = "s.young@hotmail.com", CreatedOn = new DateTime(2024, 12, 18), Bio = "Specializes in mapping historical trade routes." }, "male-profile-picture-8.jpeg"),
            (new Profile { UserName = "scarlett.adams", Email = "scarlett.adams@yahoo.com", CreatedOn = new DateTime(2024, 12, 19) }, "female-profile-picture-9.jpeg"),
            (new Profile { UserName = "alexander.mitchell", Email = "alex.mitchell@gmail.com", CreatedOn = new DateTime(2024, 12, 20), Bio = "Enjoys creating maps of national parks." }, "male-profile-picture-9.jpeg"),
            (new Profile { UserName = "luna.campbell", Email = "luna_c@aol.com", CreatedOn = new DateTime(2024, 12, 21), Bio = "Exploring the geography of islands." }, "female-profile-picture-10.jpeg"),
            (new Profile { UserName = "jackson.morris", Email = "jackson.morris@gmail.com", CreatedOn = new DateTime(2024, 12, 22) }, "male-profile-picture-10.jpeg"),
            (new Profile { UserName = "avery.rogers", Email = "avery.rogers@outlook.com", CreatedOn = new DateTime(2024, 12, 23), Bio = "Fascinated by mapping cultural landmarks." }, null),
            (new Profile { UserName = "logan.reed", Email = "logan.reed@yahoo.com", CreatedOn = new DateTime(2024, 12, 24), Bio = "Focuses on mapping urban growth." }, "male-profile-picture-11.jpeg"),
            (new Profile { UserName = "ella.cook", Email = "ella.cook@gmail.com", CreatedOn = new DateTime(2024, 12, 25), Bio = "Interested in mapping rural landscapes." }, "female-profile-picture-11.jpeg"),
            (new Profile { UserName = "daniel.morgan", Email = "daniel.morgan@gmail.com", CreatedOn = new DateTime(2024, 12, 26) }, "male-profile-picture-12.jpeg"),
            (new Profile { UserName = "grace.bell", Email = "grace.bell@yahoo.com", CreatedOn = new DateTime(2024, 12, 27), Bio = "Exploring the geography of deserts." }, "female-profile-picture-12.jpeg"),
            (new Profile { UserName = "aiden.murphy", Email = "aidenmurphy@outlook.com", CreatedOn = new DateTime(2024, 12, 28), Bio = "Enjoys creating maps of river systems." }, "male-profile-picture-13.jpeg"),
            (new Profile { UserName = "chloe.bailey", Email = "chloe_bailey@gmail.com", CreatedOn = new DateTime(2024, 12, 29) }, "female-profile-picture-13.jpeg"),
            (new Profile { UserName = "matthew.cooper", Email = "matthew.cooper@icloud.com", CreatedOn = new DateTime(2024, 12, 30), Bio = "Specializes in mapping weather patterns." }, "male-profile-picture-14.jpeg"),
            (new Profile { UserName = "zoe.richardson", Email = "zoe.richardson@gmail.com", CreatedOn = new DateTime(2024, 12, 31), Bio = "Fascinated by mapping volcanic regions." }, "female-profile-picture-14.jpeg"),
        };
    }
}

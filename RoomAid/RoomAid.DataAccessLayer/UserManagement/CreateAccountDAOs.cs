
namespace RoomAid.DataAccessLayer
{
   public class CreateAccountDAOs
    {
        public ICreateAccountDAO CreateAccountDAO { get; }
        public ICreateAccountDAO CreateMappingDAO { get; }
        public ICreateAccountDAO CreateUserDAO { get; }
        public IMapperDAO MapperDAO { get; }

        public CreateAccountDAOs(ICreateAccountDAO createAccountDAO, ICreateAccountDAO createMappingDAO, ICreateAccountDAO createUserDAO, IMapperDAO mapperDAO )
        {
            CreateAccountDAO = createAccountDAO;
            CreateMappingDAO = createMappingDAO;
            CreateUserDAO = createUserDAO;
            MapperDAO = mapperDAO;
        }
    }
}

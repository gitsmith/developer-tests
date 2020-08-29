namespace HockeyApi.Features
{
    public class TeamModel
    {
        public TeamModel(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code { get; }
        public string Name { get; }
    }
}

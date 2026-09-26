
namespace StateMachines
{
    public interface ITransition
    {
        public BaseStateConfig To {  get; }
        public BaseStateConfig From { get; }
        public EntityCondition[] Conditions { get; }

    }
}

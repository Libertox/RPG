

using Area;

namespace Entity
{
    public interface IPatrolable
    {
        public bool IsPatroling { get; }
        public PatrolArea PatrolArea { get; }


        public void SetPatroling(bool isPatroling);

    }
}

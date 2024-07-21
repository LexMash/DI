namespace BaCon
{
    public class DIInstanceBuilder<TCurrent, TTarget> : DIEntryBuilder<TCurrent, TTarget> where TCurrent : TTarget
    {
        private readonly TCurrent instance;

        public DIInstanceBuilder(DIContainer container, TCurrent instance) : base(container)
        {
            this.instance = instance;
            IsSingle = true;
        }

        protected override DIEntry<TCurrent> GetEntry() 
            => new DIEntry<TCurrent>(instance);
    }
}

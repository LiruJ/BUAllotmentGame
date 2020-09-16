namespace Assets.Scripts.Seeds
{
    public class BestWorst
    {
        #region Properties
        public float Best { get; set; }

        public float Worst { get; set; }
        #endregion

        #region Constructors
        public BestWorst(float best, float worst)
        {
            Best = best;
            Worst = worst;
        }

        public BestWorst(float both) : this(both, both) { }
        #endregion

        #region String Functions
        public override string ToString() => $"Best: {Best} Worst: {Worst}";
        #endregion
    }
}

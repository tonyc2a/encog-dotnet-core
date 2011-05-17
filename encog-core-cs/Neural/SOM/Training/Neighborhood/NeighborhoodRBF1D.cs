using Encog.MathUtil.RBF;

namespace Encog.Neural.SOM.Training.Neighborhood
{
    /// <summary>
    /// A neighborhood function based on an RBF function.
    /// </summary>
    ///
    public class NeighborhoodRBF1D : INeighborhoodFunction
    {
        /// <summary>
        /// The radial basis function (RBF) to use to calculate the training falloff
        /// from the best neuron.
        /// </summary>
        ///
        private readonly IRadialBasisFunction radial;

        /// <summary>
        /// Construct the neighborhood function with the specified radial function.
        /// Generally this will be a Gaussian function but any RBF should do.
        /// </summary>
        ///
        /// <param name="radial_0">The radial basis function to use.</param>
        public NeighborhoodRBF1D(IRadialBasisFunction radial_0)
        {
            radial = radial_0;
        }

        /// <summary>
        /// Construct a 1d neighborhood function.
        /// </summary>
        ///
        /// <param name="type">The RBF type to use.</param>
        public NeighborhoodRBF1D(RBFEnum type)
        {
            switch (type)
            {
                case RBFEnum.Gaussian:
                    radial = new GaussianFunction(1);
                    break;
                case RBFEnum.InverseMultiquadric:
                    radial = new InverseMultiquadricFunction(1);
                    break;
                case RBFEnum.Multiquadric:
                    radial = new MultiquadricFunction(1);
                    break;
                case RBFEnum.MexicanHat:
                    radial = new MexicanHatFunction(1);
                    break;
                default:
                    throw new NeuralNetworkError("Unknown RBF type: " + type);
            }

            radial.Width = 1.0d;
        }

        #region INeighborhoodFunction Members

        /// <summary>
        /// Determine how much the current neuron should be affected by training
        /// based on its proximity to the winning neuron.
        /// </summary>
        ///
        /// <param name="currentNeuron">THe current neuron being evaluated.</param>
        /// <param name="bestNeuron">The winning neuron.</param>
        /// <returns>The ratio for this neuron's adjustment.</returns>
        public virtual double Function(int currentNeuron, int bestNeuron)
        {
            var d = new double[1];
            d[0] = currentNeuron - bestNeuron;
            return radial.Calculate(d);
        }

        /// <summary>
        /// Set the radius.
        /// </summary>
        ///
        /// <value>The new radius.</value>
        public virtual double Radius
        {
            /// <returns>The radius.</returns>
            get { return radial.Width; }
            /// <summary>
            /// Set the radius.
            /// </summary>
            ///
            /// <param name="radius">The new radius.</param>
            set { radial.Width = value; }
        }

        #endregion
    }
}
namespace WpfCommonLibrary
{
    #region Usings
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    #endregion Usings

    /// <summary>
    /// Common class inherited by View Models to use common helpers methods. 
    /// </summary>
    [Serializable]
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// Occurs when a property inside the View Model is being changed. 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        /// <summary>
        /// Notifies that the specified View Model property has been updated. 
        /// </summary>
        /// <param name="propertyExpression"> An expression that references the property to notify. </param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected void Notify(Expression<Func<object>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentException();
            Notify((propertyExpression.Body as MemberExpression ?? ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression).Member.Name);
        }

        /// <summary>
        /// Notifies that the specified View Model property has been updated. 
        /// </summary>
        /// <param name="propertyName"> Name of the property. </param>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification =
                "Unfortunately there's no other way to use this attribute, but to implement a default parameter.")]
        protected void Notify([CallerMemberName] string propertyName = null)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException();

            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the property and raise the PropertyChanged event if it is updated. 
        /// </summary>
        /// <typeparam name="TProperty"> The type of the property. </typeparam>
        /// <param name="property"> The property to update. </param>
        /// <param name="value"> The value to set inside the property. </param>
        /// <param name="propertyName"> Name of the property to update. </param>
        /// <returns> Returns true if the property value has been changed, false otherwise. </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification =
                "Unfortunately there's no other way to use the CallerMemberInfo attribute. Otherwise we would get a compile error."
            ),
         SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#",
             Justification = "Maybe is not the best practice but is quite nice to use it in this way.")]
        protected bool SetProperty<TProperty>(ref TProperty property, TProperty value,
            [CallerMemberName] String propertyName = null)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException();

            if (Equals(property, value)) return false;

            property = value;

            Notify(propertyName);

            return true;
        }

        #endregion Methods
    }
}
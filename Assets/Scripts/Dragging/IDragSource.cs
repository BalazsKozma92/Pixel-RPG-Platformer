using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <typeparam name="T">The type that represents the item being dragged.</typeparam>
public interface IDragSource<T> where T : class
{
    /// <summary>
    /// What item type currently resides in this source?
    /// </summary>
    T GetItem();
    T GetAbilityItem();

    /// <summary>
    /// What is the quantity of items in this source?
    /// </summary>
    int GetNumber();

    /// <summary>
    /// Remove a given number of items from the source.
    /// </summary>
    /// <param name="number">
    /// This should never exceed the number returned by `GetNumber`.
    /// </param>
    void RemoveItems(int number);
    void RemoveAbilityItems(int number);
    void ChangeSourceAbilitySocketName();
}
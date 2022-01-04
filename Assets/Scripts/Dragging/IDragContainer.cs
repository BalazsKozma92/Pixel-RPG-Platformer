using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <typeparam name="T">The type that represents the item being dragged.</typeparam>
public interface IDragContainer<T> : IDragDestination<T>, IDragSource<T> where T : class
{
}

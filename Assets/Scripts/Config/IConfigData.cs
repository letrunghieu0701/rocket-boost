using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConfigData<TKey>
{
    TKey ID { get; }
}

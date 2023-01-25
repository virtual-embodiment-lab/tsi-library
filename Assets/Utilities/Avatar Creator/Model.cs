using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Demographics;
using System;

public class Model : MonoBehaviour
{
    [SerializeField] protected Tuple<int, int> Age = null;
    [SerializeField] protected Race[] Race = null;
    [SerializeField] protected Gender[] Gender = null;

    public Tuple<Tuple<int,int>, Race[], Gender[]> getInfo() {
        return new Tuple<Tuple<int,int>, Race[], Gender[]> (Age, Race, Gender);
    }
 }

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Models;

public class TokenModelTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void Collected_ByDefault_IsFalse()
    {
        // Arrange
        var tokenModel = new TokenModel();

        // Act
        
        // Assert
        Assert.AreEqual(false, tokenModel.Collected(Vector2.zero));
    }
}

/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{

    private AudioSource shellSoundFX;
    private Renderer shellRenderer;
    private Collider shellCollider;
    public AudioClip explosionSound;

    private GameManager m_GameManager;


    // Use this for initialization
    void Start()
    {
        shellSoundFX = gameObject.GetComponent<AudioSource>();
        shellSoundFX.Play();
        shellRenderer = gameObject.GetComponent<Renderer>();
        shellCollider = gameObject.GetComponent<Collider>();

        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (m_GameManager == null)
            Debug.LogError("m_GameManager = null");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (other.tag == "WeakWall")
        {
            m_GameManager.AddScore();
            shellSoundFX.clip = explosionSound;
            shellSoundFX.Play();
            shellCollider.enabled = false;
            shellRenderer.enabled = false;
            Destroy(gameObject, explosionSound.length);
            Destroy(other.gameObject);
        }
    }
}

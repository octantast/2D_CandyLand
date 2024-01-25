using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GeneralController : MonoBehaviour
{
    public UI ui;
    public TouchController touches;

    // backs
    public List<GameObject> backgrounds;
    public List<string> backgrounddescriptions;

    public List<GameObject> candiestospawn;
    public GameObject bombPrefab;
    public GameObject specialPrefab;
    public GameObject rainbowPrefab;

    public Sprite universalCandy;
    public List<CandyScript> allcandies;
    public List<Sprite> allsprites;
    private float spawnTimer;
    public float spawnTimerMax;
    private float lastX;
    private float lastY;
    private float randomspecial;

    public List<Vector3> platetospawn;
    public GameObject candyHolder;
    private GameObject lastcandy;

    public bool paused;
    private bool spawncandy;

    private int bombs;

    // effects

    public List<ParticleSystem> effects;
    public Animator puff;
    public void SpawnObjects(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            int randomIndex = Random.Range(0, candiestospawn.Count);
            GameObject candy = Instantiate(candiestospawn[randomIndex], transform.position, Quaternion.identity, candyHolder.transform);
            if (lastX >= 0)
            {
                lastX = Random.Range(-1.6f, -0.5f);
            }
            else if (lastX < 0)
            {
                lastX = Random.Range(0.5f, 1.6f);
            }
            candy.transform.localPosition = new Vector2(lastX, -i);

            float randomAngle = Random.Range(0f, 360f);
            candy.transform.localRotation = Quaternion.Euler(0f, 0f, randomAngle);

            candy.GetComponent<Rigidbody2D>().gravityScale = ui.gravityIndex;

            CandyScript scriptcandy = candy.GetComponent<CandyScript>();
            scriptcandy.parentobj = candyHolder;
            scriptcandy.general = this;
            scriptcandy.touch = touches;
            allcandies.Add(scriptcandy);
        }
    }
    public void CandySpawner()
    {
        // may be bomb
        if (!ui.bombsAllowed)
        {
            spawncandy = true;
           
        }
        else
        {
            bombs += 1;
            if(bombs > 3)
            {
                bombs = 0;
                randomspecial = Random.Range(0, 10);
                if (ui.specialssAllowed && randomspecial > 2)
                {
                    if (randomspecial >= 7)
                    {
                        // add 1
                        GameObject special = Instantiate(specialPrefab, transform.position, Quaternion.identity, candyHolder.transform);
                       
                        lastY = Random.Range(-3f, -6f);
                        special.transform.localPosition = new Vector2(0, lastY);
                        float randomAngle = 0;
                        special.transform.localRotation = Quaternion.Euler(0f, 0f, randomAngle);

                    }
                    else
                    {
                        // add 2
                        GameObject special = Instantiate(rainbowPrefab, transform.position, Quaternion.identity, candyHolder.transform);
                        if (lastX >= 0)
                        {
                            lastX = Random.Range(-1.6f, -0.5f);
                            lastY = Random.Range(-1f, -3f);
                        }
                        else if (lastX < 0)
                        {
                            lastX = Random.Range(0.5f, 1.6f);
                            lastY = Random.Range(-3f, -6f);
                        }
                        special.transform.localPosition = new Vector2(lastX, lastY);
                        float randomAngle = -45f;
                        special.transform.localRotation = Quaternion.Euler(0f, 0f, randomAngle);
                    }
                }
                else
                {
                    GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity, candyHolder.transform);
                    if (lastX >= 0)
                    {
                        lastX = Random.Range(-1.6f, -0.5f);
                    }
                    else if (lastX < 0)
                    {
                        lastX = Random.Range(0.5f, 1.6f);
                    }
                    bomb.transform.localPosition = new Vector2(lastX, 0);
                    float randomAngle = Random.Range(0f, 360f);
                    bomb.transform.localRotation = Quaternion.Euler(0f, 0f, randomAngle);
                    bomb.GetComponent<Rigidbody2D>().gravityScale = ui.gravityIndex;
                    BombScript scriptbomb = bomb.GetComponent<BombScript>();
                    scriptbomb.general = this;
                    scriptbomb.touch = touches;
                }
            }
            else
            {
                spawncandy = true;
               
            }
        }

        if(spawncandy)
        {
            spawncandy = false;
            int randomIndex = Random.Range(0, candiestospawn.Count);
            GameObject candy = Instantiate(candiestospawn[randomIndex], transform.position, Quaternion.identity, candyHolder.transform);
            if (lastX >= 0)
            {
                lastX = Random.Range(-1.6f, -0.5f);
            }
            else if (lastX < 0)
            {
                lastX = Random.Range(0.5f, 1.6f);
            }
            candy.transform.localPosition = new Vector2(lastX, 0);

            float randomAngle = Random.Range(0f, 360f);
            candy.transform.localRotation = Quaternion.Euler(0f, 0f, randomAngle);

            candy.GetComponent<Rigidbody2D>().gravityScale = ui.gravityIndex;

            CandyScript scriptcandy = candy.GetComponent<CandyScript>();
            scriptcandy.parentobj = candyHolder;
            scriptcandy.general = this;
            scriptcandy.touch = touches;
            allcandies.Add(scriptcandy);
        }
    }

    public void DestroyBomb(GameObject bomb)
    {
        ui.sounds[5].Play();
        Destroy(bomb);
    }

    public void destroyingEffect(GameObject candy, int howmany)
    {
        if (lastcandy != candy)
        {
            lastcandy = candy;


            // particles
            int numberOfSystems = Random.Range(1, effects.Count + 1);
            for (int i = 0; i < numberOfSystems; i++)
            {
                ParticleSystem prefab = effects[Random.Range(0, effects.Count)];
                Instantiate(prefab, candy.transform.position, Quaternion.identity);
            }

            if(candy.transform.childCount != 0)
            {
                foreach (Transform child in candy.transform)
                {
                    allcandies.Remove(child.gameObject.GetComponent<CandyScript>());
                    Destroy(child.gameObject);
                }
            }

            allcandies.Remove(candy.GetComponent<CandyScript>());
            Destroy(candy);

            // sound


            // count with bonuses
            if (howmany > 1)
            {
                ui.sounds[5].Play();
                if (!ui.a2active)
                {
                    ui.currentScore += howmany * ui.candyScoreIndex;
                }
                else
                {
                    ui.currentScore += howmany * ui.candyScoreIndex * 2;
                }
            }
        }
        else
        {
            lastcandy = null;
        }

    }

    public void puffeffect(Vector2 position)
    {
        puff.transform.position = position;
        puff.gameObject.SetActive(false);
        puff.enabled = false;
        puff.gameObject.SetActive(true);
        puff.enabled = true;
        puff.Play("puff");

        ui.sounds[3].Play();
    }

    public void destroyingEffect2(GameObject candy, GameObject bomb)
    {
        puffeffect(bomb.transform.position);
        Destroy(bomb);

        //int childCount = candy.transform.childCount;
        //int numberOfChildrenToDestroy = Mathf.Min(childCount, Random.Range(1, 3));

        //for (int i = childCount - 1; i >= childCount - numberOfChildrenToDestroy; i--)
        //{
        //    allcandies.Remove(candy.transform.GetChild(i).gameObject.GetComponent<CandyScript>());
        //Destroy(candy.transform.GetChild(i).gameObject);
        //}

        foreach (Transform child in candy.transform)
        {
            allcandies.Remove(child.gameObject.GetComponent<CandyScript>());
            Destroy(child.gameObject);
        }
        allcandies.Remove(candy.GetComponent<CandyScript>());
        Destroy(candy);
    }

   public void Update()
    {
        if (!paused)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                CandySpawner();
                spawnTimer = Random.Range(2.5f, spawnTimerMax);
            }

            // skills
            //if (ui.a1timer > 0)
            //{
            //    ui.a1timer -= Time.deltaTime;
            //    ui.a1activeskale.fillAmount = ui.a1timer / ui.a1timerMax;
            //}
            //else if(ui.a1active)
            //{
            //    ui.a1activeskale.fillAmount = 0;
            //    ui.a1active = false;
            //    foreach (CandyScript candyscript in allcandies)
            //    {
            //        candyscript.thisrenderer.sprite = candyscript.thisSprite;
            //        candyscript.gameObject.tag = candyscript.thistag;
            //    }
            //}

            if (ui.a2timer > 0)
            {
                ui.a2timer -= Time.deltaTime;
                ui.a2activeskale.fillAmount = ui.a2timer / ui.a2timerMax;
            }
            else if (ui.a2active)
            {
                ui.a2activeskale.fillAmount = 0;
                ui.a2active = false;
            }
        }
    }
 

}

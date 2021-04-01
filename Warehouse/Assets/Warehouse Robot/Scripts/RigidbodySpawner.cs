using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Perception.GroundTruth;
using RosSharp;
using Unity.Simulation.Warehouse;

public class RigidbodySpawner : MonoBehaviour
{
    public GameObject LocationPicker;
    public GameObject Robot;
    public GameObject m_box;
    private Vector3 m_boxDims;
    private int m_debrisSpawn;
    private GameObject m_spawnedBoxes;
    private static bool RobotInitiated = false;

    // Start is called before the first frame update
    void Start()
    {
        HidePicker();
    }

    public void SpawnBoxes(){
        var boxIn = transform.Find("SpawnBoxes").GetComponent<InputField>().text.Split(',');
        if (boxIn.Length != 3){
            Debug.LogError($"Invalid box input dimensions!");
            return;
        }

        if (m_spawnedBoxes != null) {
            Destroy(m_spawnedBoxes);
        }
            
        m_spawnedBoxes = new GameObject("Spawned");
        m_boxDims = new Vector3(int.Parse(boxIn[0]), int.Parse(boxIn[1]), int.Parse(boxIn[2]));
        
        var boxSize = m_box.GetComponentInChildren<Renderer>().bounds.size;
        for (int i = 0; i < m_boxDims[0]; i++){
            for (int j = 0; j < m_boxDims[1]; j++){
                for (int k = 0; k < m_boxDims[2]; k++){
                    var o = Instantiate(m_box, new Vector3(i * boxSize.x, k * boxSize.y, j * boxSize.z), Quaternion.identity, m_spawnedBoxes.transform);
                    Destroy(o.GetComponent<BoxDropoff>());
                }
            }
        }

        m_spawnedBoxes.transform.position = LocationPicker.transform.position;
    }

    public void SpawnDebris(){
        m_debrisSpawn = int.Parse(transform.Find("SpawnDebris").GetComponent<InputField>().text);

        if (m_spawnedBoxes != null) {
            Destroy(m_spawnedBoxes);
        }
            
        m_spawnedBoxes = new GameObject("Spawned");

        var size = GameObject.FindObjectOfType<WarehouseManager>().GetEditorParams().m_debrisSize;

        for (int i = 0; i < m_debrisSpawn; i++){
            var randShape = UnityEngine.Random.Range(0, 4);
            GameObject obj;
            switch(randShape){
                case 0:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                case 1:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
                case 2:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    break;
                case 3:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
                default:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                
            }
            var pos = new Vector3(UnityEngine.Random.Range(-size * 5,size * 5), UnityEngine.Random.Range(0.01f, 1f), UnityEngine.Random.Range(-size * 5,size * 5));
            
            obj.transform.localScale = new Vector3(UnityEngine.Random.Range(0.005f, size), UnityEngine.Random.Range(0.005f, size), UnityEngine.Random.Range(0.005f, size));
            obj.transform.parent = m_spawnedBoxes.transform;
            obj.transform.localPosition = pos;
            obj.transform.rotation = UnityEngine.Random.rotation;
            obj.GetComponent<Renderer>().material = Resources.Load<Material>($"Materials/Debris");

            var lab = obj.AddComponent<Labeling>();
            lab.labels.Add("debris");

            var rb = obj.AddComponent<Rigidbody>();
        }

        m_spawnedBoxes.transform.position = LocationPicker.transform.position;
    }

    public void ShowRobot()
    {
        // This function doesn't look like it's getting called anywhere, so commenting out for now...
//        if (!RobotInitiated)
//        {
//            ImportSettings settings = ImportSettings.DefaultSettings();
//            GameObject rob = RosSharp.Urdf.Editor.UrdfRobotExtensions.Create("/Users/vidur.vij/Desktop/Robotics-AMR-Spike/Warehouse_2020.2/Assets/turtlebot3/model.urdf", settings);
//            if (rob == null)
//                Debug.Log("nope");
//            //Tile Constraint
//            GameObject tiltConstraint = new GameObject("TiltConstraint");
//            //tiltConstraint.transform.parent = rob.transform;
//            ConfigurableJoint constraint = tiltConstraint.AddComponent<ConfigurableJoint>();
//            tiltConstraint.GetComponent<Rigidbody>().isKinematic = true;
//            constraint.angularXMotion = ConfigurableJointMotion.Locked;
//            constraint.angularZMotion = ConfigurableJointMotion.Locked;
//            GameObject baseLink = GameObject.Find("base_link");
//            constraint.connectedArticulationBody = baseLink.GetComponent<ArticulationBody>();
//
//            //Remove Collisions
//            Collider[] casterCollider = GameObject.Find("caster_back_right_link").GetComponentsInChildren<Collider>();
//            foreach (Collider col in casterCollider)
//                DestroyImmediate(col);
//
//            Collider[] casterCollider2 = GameObject.Find("caster_back_left_link").GetComponentsInChildren<Collider>();
//            foreach (Collider col in casterCollider2)
//                DestroyImmediate(col);
//            //DestroyController
//            DestroyImmediate(rob.GetComponent<RosSharp.Control.Controller>());
//            //TensorUpdate
//            Vector3 inertiaup = new Vector3(1, 1, 1);
//            GameObject wheel1 = GameObject.Find("wheel_left_link").gameObject;
//            GameObject wheel2 = GameObject.Find("wheel_right_link").gameObject;
//            //DestroyImmediate(wheel1.GetComponent<JointControl>());
//            //DestroyImmediate(wheel2.GetComponent<JointControl>());
//            InertiaTensorUpdate wheel1up = wheel1.gameObject.AddComponent<InertiaTensorUpdate>();
//            InertiaTensorUpdate wheel2up = wheel2.gameObject.AddComponent<InertiaTensorUpdate>();
//            wheel1up.inertiaTensor = wheel2up.inertiaTensor = inertiaup;
//
//            //AddController
//            RosSharp.Control.Goal goal = rob.AddComponent<RosSharp.Control.Goal>();
//            RosSharp.Control.AGVController controller = rob.AddComponent<RosSharp.Control.AGVController>();
//            controller.wheel1 = wheel1;
//            controller.wheel2 = wheel2;
//            controller.goalFunc = goal;
//            controller.centerPoint = baseLink;
//            controller.forceLimit = 100;
//            controller.damping = 100;
//            controller.maxLinearSpeed = .2f;
//            controller.maxRotationalSpeed = .2f;
//            Robot = rob;
//            RobotInitiated = true;
//        }
//
//        Robot.transform.position = LocationPicker.transform.position;
//        Debug.Log(Robot.transform.position);
//        Debug.Log(LocationPicker.transform.position);
    }

    public void ShowPicker(){
        LocationPicker.SetActive(true);
    }

    public void HidePicker(){
        LocationPicker.SetActive(false);
    }
};
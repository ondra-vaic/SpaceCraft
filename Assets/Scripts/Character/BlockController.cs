using UnityEngine;
using UnityEngine.InputSystem;

public class BlockController : MonoBehaviour
{
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private Transform gizmoBlock;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;
    [SerializeField] private Camera cam;
    [SerializeField] private float strength;
    
    private BlockType blockInHand = BlockType.DIRT;
    
    private void Update()
    {
        handleMouseClicks();
        placeGizmo();
    }

    private void placeGizmo()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            Vector3 point = hit.point - hit.normal *.5f;
            gizmoBlock.transform.position = new Vector3Int((int) point.x, (int) point.y, (int) point.z) + Vector3.one * 0.5f + hit.normal;
            
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
        }
        else
        {
            gizmoBlock.transform.position = cam.transform.position - cam.transform.forward * 10;
        }
    }

    private void handleMouseClicks()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            placeBlock();
        }
        else if (Mouse.current.rightButton.isPressed)
        {
            hitBlock();
        }
    }

    private void placeBlock()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            mapGenerator.PlaceBlock(hit.point + hit.normal * .5f, blockInHand);  
        }
    }
    
    private void hitBlock()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            BlockType blockType = mapGenerator.HitBlock(hit.point - hit.normal * .5f, strength);
            if (blockType != BlockType.UNKNOWN)
            {
                blockInHand = blockType;
            }
        }
    }
}

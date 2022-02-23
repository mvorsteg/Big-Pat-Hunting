// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""7d17686f-c243-46e5-94b4-41d4d8bfda7d"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8256ec7c-134a-4424-8724-7b2745cffa88"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""092f70ea-28a8-4f5b-a01b-5415b55bfaac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""21f8ca59-cfc4-4962-bb24-463a3d5669f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""b737006d-34e3-4257-9a7f-8b093c43b543"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e1ada21c-32d2-44d2-b4b7-59da8dc583de"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""316e0f3b-73d5-4c10-aaf7-1c976053a0e1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b79e747b-9ef5-42ea-abcc-643e5f66de58"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b561bf50-2b76-44b6-9537-3721e475a27d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""960715d3-d938-4ab0-af9a-89a2838e1a9d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""438f8895-242e-4547-9d10-7f81741749cf"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""FPS"",
            ""id"": ""74a26f2b-8764-41af-837e-6668c97245b5"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""53bde74c-8ac2-44a8-b982-57c3057b8de6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""ddb333ae-71e7-473a-9405-0656a081ae31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""5ed4782a-f485-4358-be08-6222c021ae1e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""920bcfcb-7984-4af2-bfac-42b34a2a3ea3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Use"",
                    ""type"": ""Button"",
                    ""id"": ""6f28aca6-e833-4b21-9998-40b8c687c999"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""e17b8448-3e6f-46e1-ad1a-7a1e8b2aafa2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""691ac177-da22-4015-8616-da26d7ad9b2e"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a07591d4-d4bf-4f44-a568-44b9be864824"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d57996ba-c1d6-4413-beb4-b2aec150e721"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c0b6fa8-71c1-4e06-87a5-acadf803aea8"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea8ed64c-c87d-468d-8b99-31cdcb1b6840"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8499d3c-3846-468f-a4dd-ea7dee18e497"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CameraOnly"",
            ""id"": ""6f1b1db1-58f3-4f7a-9520-3d177ca92f63"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d8e2d5c7-6677-426d-b121-e6444cb10b8e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""3865e3ce-07ca-4a24-a1f8-d2c0d9697b55"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""544914ac-d474-40de-ac61-ae797d47f7ae"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ece5f57-ae31-4dc7-92b8-4832230639ea"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""AnyKey"",
            ""id"": ""af6b064b-8ba3-4b16-9148-bd473b652639"",
            ""actions"": [
                {
                    ""name"": ""AnyKey"",
                    ""type"": ""Button"",
                    ""id"": ""a19cc95e-bc56-449c-afca-d754fde2bc6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a3ec82f0-0569-4cb6-9b62-fb933ef735c7"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AnyKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b570638-ef14-4e7d-a7ea-8eb2b3751ade"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AnyKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7585efb7-2fc6-43c8-97b0-c8590ec3ad73"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AnyKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PauseMenu"",
            ""id"": ""7a8d9470-5b31-4929-bcb6-9b081b8ff300"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""2f9e8dbc-76c8-4e87-a352-b172c3ad2747"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""675fc9d0-9b2f-41df-995b-2d5f497080a1"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""1103d467-1aee-41f2-9227-73b231872b83"",
            ""actions"": [
                {
                    ""name"": ""KillYourself"",
                    ""type"": ""Button"",
                    ""id"": ""9f81f1dc-8f00-4a45-b4a7-a7614a0f68d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9623a7f6-5e71-4fb1-89ed-c9c68fd9e206"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KillYourself"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        m_Movement_Jump = m_Movement.FindAction("Jump", throwIfNotFound: true);
        m_Movement_Sprint = m_Movement.FindAction("Sprint", throwIfNotFound: true);
        // FPS
        m_FPS = asset.FindActionMap("FPS", throwIfNotFound: true);
        m_FPS_Look = m_FPS.FindAction("Look", throwIfNotFound: true);
        m_FPS_Shoot = m_FPS.FindAction("Shoot", throwIfNotFound: true);
        m_FPS_Aim = m_FPS.FindAction("Aim", throwIfNotFound: true);
        m_FPS_Reload = m_FPS.FindAction("Reload", throwIfNotFound: true);
        m_FPS_Use = m_FPS.FindAction("Use", throwIfNotFound: true);
        m_FPS_Inventory = m_FPS.FindAction("Inventory", throwIfNotFound: true);
        // CameraOnly
        m_CameraOnly = asset.FindActionMap("CameraOnly", throwIfNotFound: true);
        m_CameraOnly_Look = m_CameraOnly.FindAction("Look", throwIfNotFound: true);
        m_CameraOnly_Inventory = m_CameraOnly.FindAction("Inventory", throwIfNotFound: true);
        // AnyKey
        m_AnyKey = asset.FindActionMap("AnyKey", throwIfNotFound: true);
        m_AnyKey_AnyKey = m_AnyKey.FindAction("AnyKey", throwIfNotFound: true);
        // PauseMenu
        m_PauseMenu = asset.FindActionMap("PauseMenu", throwIfNotFound: true);
        m_PauseMenu_Pause = m_PauseMenu.FindAction("Pause", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_KillYourself = m_Debug.FindAction("KillYourself", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Move;
    private readonly InputAction m_Movement_Jump;
    private readonly InputAction m_Movement_Sprint;
    public struct MovementActions
    {
        private @PlayerControls m_Wrapper;
        public MovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputAction @Jump => m_Wrapper.m_Movement_Jump;
        public InputAction @Sprint => m_Wrapper.m_Movement_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                @Sprint.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // FPS
    private readonly InputActionMap m_FPS;
    private IFPSActions m_FPSActionsCallbackInterface;
    private readonly InputAction m_FPS_Look;
    private readonly InputAction m_FPS_Shoot;
    private readonly InputAction m_FPS_Aim;
    private readonly InputAction m_FPS_Reload;
    private readonly InputAction m_FPS_Use;
    private readonly InputAction m_FPS_Inventory;
    public struct FPSActions
    {
        private @PlayerControls m_Wrapper;
        public FPSActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_FPS_Look;
        public InputAction @Shoot => m_Wrapper.m_FPS_Shoot;
        public InputAction @Aim => m_Wrapper.m_FPS_Aim;
        public InputAction @Reload => m_Wrapper.m_FPS_Reload;
        public InputAction @Use => m_Wrapper.m_FPS_Use;
        public InputAction @Inventory => m_Wrapper.m_FPS_Inventory;
        public InputActionMap Get() { return m_Wrapper.m_FPS; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FPSActions set) { return set.Get(); }
        public void SetCallbacks(IFPSActions instance)
        {
            if (m_Wrapper.m_FPSActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_FPSActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_FPSActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_FPSActionsCallbackInterface.OnLook;
                @Shoot.started -= m_Wrapper.m_FPSActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_FPSActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_FPSActionsCallbackInterface.OnShoot;
                @Aim.started -= m_Wrapper.m_FPSActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_FPSActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_FPSActionsCallbackInterface.OnAim;
                @Reload.started -= m_Wrapper.m_FPSActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_FPSActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_FPSActionsCallbackInterface.OnReload;
                @Use.started -= m_Wrapper.m_FPSActionsCallbackInterface.OnUse;
                @Use.performed -= m_Wrapper.m_FPSActionsCallbackInterface.OnUse;
                @Use.canceled -= m_Wrapper.m_FPSActionsCallbackInterface.OnUse;
                @Inventory.started -= m_Wrapper.m_FPSActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_FPSActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_FPSActionsCallbackInterface.OnInventory;
            }
            m_Wrapper.m_FPSActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @Use.started += instance.OnUse;
                @Use.performed += instance.OnUse;
                @Use.canceled += instance.OnUse;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
            }
        }
    }
    public FPSActions @FPS => new FPSActions(this);

    // CameraOnly
    private readonly InputActionMap m_CameraOnly;
    private ICameraOnlyActions m_CameraOnlyActionsCallbackInterface;
    private readonly InputAction m_CameraOnly_Look;
    private readonly InputAction m_CameraOnly_Inventory;
    public struct CameraOnlyActions
    {
        private @PlayerControls m_Wrapper;
        public CameraOnlyActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_CameraOnly_Look;
        public InputAction @Inventory => m_Wrapper.m_CameraOnly_Inventory;
        public InputActionMap Get() { return m_Wrapper.m_CameraOnly; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraOnlyActions set) { return set.Get(); }
        public void SetCallbacks(ICameraOnlyActions instance)
        {
            if (m_Wrapper.m_CameraOnlyActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_CameraOnlyActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_CameraOnlyActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_CameraOnlyActionsCallbackInterface.OnLook;
                @Inventory.started -= m_Wrapper.m_CameraOnlyActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_CameraOnlyActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_CameraOnlyActionsCallbackInterface.OnInventory;
            }
            m_Wrapper.m_CameraOnlyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
            }
        }
    }
    public CameraOnlyActions @CameraOnly => new CameraOnlyActions(this);

    // AnyKey
    private readonly InputActionMap m_AnyKey;
    private IAnyKeyActions m_AnyKeyActionsCallbackInterface;
    private readonly InputAction m_AnyKey_AnyKey;
    public struct AnyKeyActions
    {
        private @PlayerControls m_Wrapper;
        public AnyKeyActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @AnyKey => m_Wrapper.m_AnyKey_AnyKey;
        public InputActionMap Get() { return m_Wrapper.m_AnyKey; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AnyKeyActions set) { return set.Get(); }
        public void SetCallbacks(IAnyKeyActions instance)
        {
            if (m_Wrapper.m_AnyKeyActionsCallbackInterface != null)
            {
                @AnyKey.started -= m_Wrapper.m_AnyKeyActionsCallbackInterface.OnAnyKey;
                @AnyKey.performed -= m_Wrapper.m_AnyKeyActionsCallbackInterface.OnAnyKey;
                @AnyKey.canceled -= m_Wrapper.m_AnyKeyActionsCallbackInterface.OnAnyKey;
            }
            m_Wrapper.m_AnyKeyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AnyKey.started += instance.OnAnyKey;
                @AnyKey.performed += instance.OnAnyKey;
                @AnyKey.canceled += instance.OnAnyKey;
            }
        }
    }
    public AnyKeyActions @AnyKey => new AnyKeyActions(this);

    // PauseMenu
    private readonly InputActionMap m_PauseMenu;
    private IPauseMenuActions m_PauseMenuActionsCallbackInterface;
    private readonly InputAction m_PauseMenu_Pause;
    public struct PauseMenuActions
    {
        private @PlayerControls m_Wrapper;
        public PauseMenuActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_PauseMenu_Pause;
        public InputActionMap Get() { return m_Wrapper.m_PauseMenu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PauseMenuActions set) { return set.Get(); }
        public void SetCallbacks(IPauseMenuActions instance)
        {
            if (m_Wrapper.m_PauseMenuActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_PauseMenuActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PauseMenuActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PauseMenuActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_PauseMenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public PauseMenuActions @PauseMenu => new PauseMenuActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_KillYourself;
    public struct DebugActions
    {
        private @PlayerControls m_Wrapper;
        public DebugActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @KillYourself => m_Wrapper.m_Debug_KillYourself;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @KillYourself.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnKillYourself;
                @KillYourself.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnKillYourself;
                @KillYourself.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnKillYourself;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @KillYourself.started += instance.OnKillYourself;
                @KillYourself.performed += instance.OnKillYourself;
                @KillYourself.canceled += instance.OnKillYourself;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface IFPSActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnUse(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
    }
    public interface ICameraOnlyActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
    }
    public interface IAnyKeyActions
    {
        void OnAnyKey(InputAction.CallbackContext context);
    }
    public interface IPauseMenuActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnKillYourself(InputAction.CallbackContext context);
    }
}

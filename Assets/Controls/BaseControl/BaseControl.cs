//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Controls/BaseController/BaseControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @BaseControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @BaseControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BaseControl"",
    ""maps"": [
        {
            ""name"": ""Battlefield"",
            ""id"": ""07bcca95-5aef-41d7-bf27-92656763fed6"",
            ""actions"": [
                {
                    ""name"": ""Target Choosing"",
                    ""type"": ""Button"",
                    ""id"": ""783d09aa-7b16-4e90-a91e-543031062f6f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Target Canceling"",
                    ""type"": ""Button"",
                    ""id"": ""3b4d1b97-789a-450f-824d-566b3630bca6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mouse Position"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8a849411-d090-45a1-8e58-9244070c68e3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ec7afe38-05e7-4662-94d9-d9812c34e6b7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Target Choosing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd21742b-dc30-4918-891a-088b51a1cc5f"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Target Canceling"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""194b016a-a888-46cf-afdb-6a9990539b81"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Target Canceling"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c229b3a2-c4c4-49a6-841d-e0e94f0de779"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Battlefield
        m_Battlefield = asset.FindActionMap("Battlefield", throwIfNotFound: true);
        m_Battlefield_TargetChoosing = m_Battlefield.FindAction("Target Choosing", throwIfNotFound: true);
        m_Battlefield_TargetCanceling = m_Battlefield.FindAction("Target Canceling", throwIfNotFound: true);
        m_Battlefield_MousePosition = m_Battlefield.FindAction("Mouse Position", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Battlefield
    private readonly InputActionMap m_Battlefield;
    private IBattlefieldActions m_BattlefieldActionsCallbackInterface;
    private readonly InputAction m_Battlefield_TargetChoosing;
    private readonly InputAction m_Battlefield_TargetCanceling;
    private readonly InputAction m_Battlefield_MousePosition;
    public struct BattlefieldActions
    {
        private @BaseControl m_Wrapper;
        public BattlefieldActions(@BaseControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @TargetChoosing => m_Wrapper.m_Battlefield_TargetChoosing;
        public InputAction @TargetCanceling => m_Wrapper.m_Battlefield_TargetCanceling;
        public InputAction @MousePosition => m_Wrapper.m_Battlefield_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_Battlefield; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattlefieldActions set) { return set.Get(); }
        public void SetCallbacks(IBattlefieldActions instance)
        {
            if (m_Wrapper.m_BattlefieldActionsCallbackInterface != null)
            {
                @TargetChoosing.started -= m_Wrapper.m_BattlefieldActionsCallbackInterface.OnTargetChoosing;
                @TargetChoosing.performed -= m_Wrapper.m_BattlefieldActionsCallbackInterface.OnTargetChoosing;
                @TargetChoosing.canceled -= m_Wrapper.m_BattlefieldActionsCallbackInterface.OnTargetChoosing;
                @TargetCanceling.started -= m_Wrapper.m_BattlefieldActionsCallbackInterface.OnTargetCanceling;
                @TargetCanceling.performed -= m_Wrapper.m_BattlefieldActionsCallbackInterface.OnTargetCanceling;
                @TargetCanceling.canceled -= m_Wrapper.m_BattlefieldActionsCallbackInterface.OnTargetCanceling;
                @MousePosition.started -= m_Wrapper.m_BattlefieldActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_BattlefieldActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_BattlefieldActionsCallbackInterface.OnMousePosition;
            }
            m_Wrapper.m_BattlefieldActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TargetChoosing.started += instance.OnTargetChoosing;
                @TargetChoosing.performed += instance.OnTargetChoosing;
                @TargetChoosing.canceled += instance.OnTargetChoosing;
                @TargetCanceling.started += instance.OnTargetCanceling;
                @TargetCanceling.performed += instance.OnTargetCanceling;
                @TargetCanceling.canceled += instance.OnTargetCanceling;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
            }
        }
    }
    public BattlefieldActions @Battlefield => new BattlefieldActions(this);
    public interface IBattlefieldActions
    {
        void OnTargetChoosing(InputAction.CallbackContext context);
        void OnTargetCanceling(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
}

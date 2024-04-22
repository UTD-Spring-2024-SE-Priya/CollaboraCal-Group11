import React, { useEffect, useState } from 'react';
import './LoginSignUp.css';
import { useNavigate } from 'react-router-dom'; // Import useNavigate from react-router-dom
import user_icon from '../Assets/person.png';
import email_icon from '../Assets/email.png';
import password_icon from '../Assets/password.png';
import * as req from '../../Requests';


const LoginSignUp = () => {
    const [action, setAction] = useState("Login");
    const [name, setName] = useState(""); // State to track name input value
    const [email, setEmail] = useState(""); // State to track email input value
    const [password, setPassword] = useState(""); // State to track password input value
    const [error, setError] = useState(""); // State to track error message
    const navigate = useNavigate(); // Initialize useNavigate

    useEffect(() => {
        // Check for prior login
        if (req.doesCookieExist()) {
            let header = req.createAuthHeaders()
            req.getRequest("/validate", header)
                .then(onValidateResponse)
                .catch(r => console.log(r))
        }
    }, [])

    // HTTP Event Handlers

    const onLoginResponse = async (response) => {
        if (response.status === 200) {
            let json = await response.json()
            console.log(json)
            req.setAuthenticationCookie(json)
            navigate("/home")
        }
        else {
            // ERROR CODE
            req.deleteAuthenticationCookie()
        }
    }

    const onValidateResponse = async (response) => {
        if (response.status == 200) {
            navigate("/home")
        }
    }




    // Functionality

    const handleToggleClick = () => {
        setAction(prevAction => prevAction === "Login" ? "Sign Up" : "Login");
        // Reset error message when toggling between Login and Sign Up
        setError("");
    };

    const validatePassword = (password) => {
        // Password should be at least 8 characters long and contain at least 1 uppercase, 1 lowercase, and 1 number
        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/;
        return passwordRegex.test(password);
    };

    const handleSubmitClick = () => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (action === "Login") {
            // For login, just check if email and password are provided
            if (!email || !password) {
                setError("Please fill in all fields.");
            } else {
                // Redirect to the "/home" route

                // REQUEST: /login
                let headers = req.createLoginHeaders(email, password)

                console.log("doing your mom " + headers);
                req.postRequest("/login", headers, {})
                    .then(onLoginResponse)
                    .catch(rejected => console.log(rejected))


                // navigate("/home");
            }
        } else {
            // For sign up, check if all fields are filled, validate email and password
            if (!name || !email || !password) {
                setError("Please fill in all fields.");
            } else if (!emailRegex.test(email)) {
                setError("Please enter a valid email.");
            } else if (!validatePassword(password)) {
                setError("Password should be at least 8 characters long with at least 1 uppercase, 1 lowercase, and 1 number.");
            } else {
                // Redirect to the "/home" route

                // REQUEST: /newuser
                let headers = req.createLoginHeaders(email, password)
                req.postRequest("/newuser", headers, name)
                    .then(onLoginResponse)
                    .catch(rejected => console.log(rejected))
            }
        }
    };

    const handleForgotPasswordClick = () => {
        navigate("/reset-password");
    };

    return (
        <div className='container'>
            <div className="header">
                <div className="text">{action}</div>
                <div className="underline"></div>
            </div>
            <div className="inputs">
                {action === "Login" ? null :
                    <div className="input">
                        <img src={user_icon} alt="" />
                        <input type="text" placeholder='Name' value={name} onChange={(e) => setName(e.target.value)} />
                    </div>
                }
                <div className="input">
                    <img src={email_icon} alt="" />
                    <input type="email" placeholder='Email ID' value={email} onChange={(e) => setEmail(e.target.value)} />
                </div>
                <div className="input">
                    <img src={password_icon} alt="" />
                    <input type="password" placeholder='Password' value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
            </div>
            {action === "Login" && <div className="forgot-password" onClick={handleForgotPasswordClick}>Forgot Password? <span>Click Here!</span></div>}
            {error && <div className="error-message errormessage">{error}</div>}
            <div className="submit-container">
                <div className="submit" onClick={handleToggleClick}>{action === "Login" ? "Sign Up" : "Login"}</div>
                <div className={`submit ${error ? 'error' : ''}`} onClick={handleSubmitClick}>Submit</div>
            </div>
            <div className="website-name">
                CollaboraCal
            </div>
        </div>
    );
};

export default LoginSignUp;
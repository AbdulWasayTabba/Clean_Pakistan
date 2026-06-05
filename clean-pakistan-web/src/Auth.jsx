import { useState } from 'react';

export default function AuthPage({ onLoginSuccess }) {
    // view can be: 'login', 'signup', or 'otp'
    const [view, setView] = useState('login');
    const [formData, setFormData] = useState({ fullName: '', phoneNumber: '', email: '', password: '', otp: '' });
    const [message, setMessage] = useState('');

    const handleChange = (e) => setFormData({ ...formData, [e.target.name]: e.target.value });

    const handleRegister = async (e) => {
        e.preventDefault();
        setMessage('Creating account...');
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Users/register`, {
                method: 'POST', headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ fullName: formData.fullName, phoneNumber: formData.phoneNumber, email: formData.email, password: formData.password })
            });
            if (response.ok) {
                setView('otp');
                setMessage('Check your email for the OTP!');
            } else {
                const error = await response.text();
                setMessage(error || 'Registration failed.');
            }
        } catch (err) { setMessage('Network error.'); }
    };

    const handleVerifyOtp = async (e) => {
        e.preventDefault();
        setMessage('Verifying...');
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Users/verify-email`, {
                method: 'POST', headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email: formData.email, otp: formData.otp })
            });
            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('cleanPakistanToken', data.token);
                onLoginSuccess();
            } else { setMessage('Invalid OTP.'); }
        } catch (err) { setMessage('Network error.'); }
    };

    const handleLogin = async (e) => {
        e.preventDefault();
        setMessage('Logging in...');
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Users/login`, {
                method: 'POST', headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email: formData.email, password: formData.password })
            });
            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('cleanPakistanToken', data.token);
                onLoginSuccess();
            } else {
                const error = await response.text();
                setMessage(error || 'Invalid credentials.');
            }
        } catch (err) { setMessage('Network error.'); }
    };

    return (
        <div style={{ maxWidth: '450px', margin: '80px auto', padding: '40px', backgroundColor: '#1a1a1a', color: 'white', borderRadius: '12px', boxShadow: '0 4px 20px rgba(0,0,0,0.5)', fontFamily: 'sans-serif' }}>
            <h2 style={{ textAlign: 'center', marginBottom: '10px' }}>
                {view === 'login' && 'Welcome Back'}
                {view === 'signup' && 'Join Clean Pakistan'}
                {view === 'otp' && 'Verify Your Email'}
            </h2>
            <p style={{ textAlign: 'center', color: '#00ff88', marginBottom: '20px', minHeight: '20px' }}>{message}</p>

            {/* --- OTP VERIFICATION SCREEN --- */}
            {view === 'otp' && (
                <form onSubmit={handleVerifyOtp} style={{ display: 'flex', flexDirection: 'column', gap: '15px' }}>
                    <p style={{ textAlign: 'center', fontSize: '14px', margin: '0' }}>Sent to: {formData.email}</p>
                    <input type="text" name="otp" placeholder="Enter 6-Digit OTP" value={formData.otp} onChange={handleChange} required style={inputStyle} />
                    <button type="submit" style={btnStyle}>Verify & Enter</button>
                </form>
            )}

            {/* --- SIGN UP SCREEN --- */}
            {view === 'signup' && (
                <form onSubmit={handleRegister} style={{ display: 'flex', flexDirection: 'column', gap: '15px' }}>
                    <input type="text" name="fullName" placeholder="Full Name" value={formData.fullName} onChange={handleChange} required style={inputStyle} />
                    <input type="tel" name="phoneNumber" placeholder="Phone Number" value={formData.phoneNumber} onChange={handleChange} required style={inputStyle} />
                    <input type="email" name="email" placeholder="Email Address" value={formData.email} onChange={handleChange} required style={inputStyle} />
                    <input type="password" name="password" placeholder="Create Password" value={formData.password} onChange={handleChange} required style={inputStyle} />
                    <button type="submit" style={btnStyle}>Create Account</button>
                </form>
            )}

            {/* --- LOGIN SCREEN --- */}
            {view === 'login' && (
                <form onSubmit={handleLogin} style={{ display: 'flex', flexDirection: 'column', gap: '15px' }}>
                    <input type="email" name="email" placeholder="Email Address" value={formData.email} onChange={handleChange} required style={inputStyle} />
                    <input type="password" name="password" placeholder="Password" value={formData.password} onChange={handleChange} required style={inputStyle} />
                    <button type="submit" style={btnStyle}>Log In</button>
                </form>
            )}

            {view !== 'otp' && (
                <p style={{ textAlign: 'center', marginTop: '20px', fontSize: '14px', color: '#aaa' }}>
                    {view === 'login' ? "Don't have an account? " : "Already have an account? "}
                    <span onClick={() => setView(view === 'login' ? 'signup' : 'login')} style={{ color: '#00ff88', cursor: 'pointer', textDecoration: 'underline' }}>
                        {view === 'login' ? 'Sign Up' : 'Log In'}
                    </span>
                </p>
            )}
        </div>
    );
}

const inputStyle = { padding: '12px', borderRadius: '6px', border: '1px solid #444', backgroundColor: '#333', color: 'white', fontSize: '15px' };
const btnStyle = { padding: '12px', backgroundColor: '#00ff88', color: '#000', border: 'none', borderRadius: '6px', fontSize: '16px', fontWeight: 'bold', cursor: 'pointer', marginTop: '10px' };
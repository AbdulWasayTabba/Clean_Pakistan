import { useState } from 'react';

export default function Login({ onLoginSuccess }) {
    const [step, setStep] = useState(1); // Step 1: Email, Step 2: OTP
    const [email, setEmail] = useState('');
    const [otp, setOtp] = useState('');
    const [message, setMessage] = useState('');

    const handleRequestOtp = async (e) => {
        e.preventDefault();
        setMessage('Sending...');

        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Users/request-otp`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email })
            });

            if (response.ok) {
                setStep(2);
                setMessage('OTP sent! Check your terminal.');
            } else {
                setMessage('Failed to send OTP.');
            }
        } catch (error) {
            setMessage('Network error. Is the server running?');
        }
    };

    const handleVerifyOtp = async (e) => {
        e.preventDefault();
        setMessage('Verifying...');

        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Users/verify-otp`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, otp })
            });

            const data = await response.json();

            if (response.ok) {
                // SECURE VAULT: Save the VIP badge to the browser!
                localStorage.setItem('cleanPakistanToken', data.token);
                onLoginSuccess(); // Tell the main app we are in!
            } else {
                setMessage(data || 'Invalid OTP.');
            }
        } catch (error) {
            setMessage('Error verifying OTP.');
        }
    };

    return (
        <div style={{ maxWidth: '400px', margin: '100px auto', padding: '30px', border: '1px solid #444', borderRadius: '10px', textAlign: 'center' }}>
            <h2>Clean Pakistan Login</h2>
            <p style={{ color: '#00ff88' }}>{message}</p>

            {step === 1 ? (
                <form onSubmit={handleRequestOtp}>
                    <input
                        type="email"
                        placeholder="Enter your email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                        style={{ width: '100%', padding: '10px', marginBottom: '15px' }}
                    />
                    <button type="submit" style={{ width: '100%', padding: '10px', cursor: 'pointer' }}>Send OTP</button>
                </form>
            ) : (
                <form onSubmit={handleVerifyOtp}>
                    <p>Email: {email}</p>
                    <input
                        type="text"
                        placeholder="Enter 6-digit OTP"
                        value={otp}
                        onChange={(e) => setOtp(e.target.value)}
                        required
                        style={{ width: '100%', padding: '10px', marginBottom: '15px' }}
                    />
                    <button type="submit" style={{ width: '100%', padding: '10px', cursor: 'pointer' }}>Verify & Login</button>
                </form>
            )}
        </div>
    );
}
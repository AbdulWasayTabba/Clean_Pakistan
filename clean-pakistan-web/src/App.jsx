import { useState, useEffect } from 'react';
import Login from './Login'; // Import your new component

function App() {
    // Check if we already have a token saved from a previous session
    const [isAuthenticated, setIsAuthenticated] = useState(!!localStorage.getItem('cleanPakistanToken'));
    const [issues, setIssues] = useState([]);

    useEffect(() => {
        // Only fetch the dashboard data if they are logged in
        if (isAuthenticated) {
            fetch(`${import.meta.env.VITE_API_URL}/api/CivicIssues`)
                .then(response => response.json())
                .then(data => setIssues(data))
                .catch(error => console.error("Error fetching data:", error));
        }
    }, [isAuthenticated]); // Re-run this if their auth status changes

    const handleLogout = () => {
        localStorage.removeItem('cleanPakistanToken'); // Shred the VIP badge
        setIsAuthenticated(false);
    };

    // THE BOUNCER: If not authenticated, show the Login screen!
    if (!isAuthenticated) {
        return <Login onLoginSuccess={() => setIsAuthenticated(true)} />;
    }

    // If they are authenticated, show the normal Dashboard
    return (
        <div style={{ padding: '40px', fontFamily: 'sans-serif' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <h1>Clean Pakistan - Admin Dashboard</h1>
                <button onClick={handleLogout} style={{ padding: '10px', background: '#ff4444', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer' }}>
                    Logout
                </button>
            </div>

            {issues.length === 0 ? (
                <p>Loading reports from the API...</p>
            ) : (
                <div style={{ display: 'grid', gap: '15px' }}>
                    {issues.map(issue => (
                        <div key={issue.id} style={{ border: '1px solid #ccc', padding: '15px', borderRadius: '8px' }}>
                            <h2 style={{ margin: '0 0 10px 0' }}>{issue.title}</h2>
                            <p><strong>Category:</strong> {issue.category}</p>
                            <p><strong>Status:</strong> {issue.status}</p>
                            <p><strong>Description:</strong> {issue.description}</p>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default App;
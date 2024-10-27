import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Form, Button, Alert, Container, Row, Col } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom'; // Import the useNavigate hook for redirection
import './Profile.css'; // Import custom CSS for additional styles

function Profile() {
  const [profile, setProfile] = useState({
    nom: '',
    email: '',
    motDePasse: '',
    telephone: '',
  });

  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const navigate = useNavigate();

  useEffect(() => {
    const fetchProfile = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setError('User not authenticated. Please log in.');
        navigate('/login');
        return;
      }

      try {
        const response = await axios.get('https://localhost:7126/api/Utilisateur', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setProfile(response.data);
      } catch (error) {
        setError('Error fetching profile data. Please log in again.');
        console.error('Error fetching profile data', error);
      }
    };

    fetchProfile();
  }, [navigate]);

  const handleSubmit = (e) => {
    e.preventDefault();

    const token = localStorage.getItem('token');
    if (!token) {
      setError('User not authenticated. Please log in.');
      navigate('/login');
      return;
    }

    setError('');
    setSuccess('');

    axios.put('https://localhost:7126/api/Utilisateur/profile', profile, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
      .then(response => {
        setSuccess('Profile updated successfully!');
        setError('');
      })
      .catch(error => {
        setError('Error updating profile. Please try again.');
        console.error('Error updating profile', error);
      });
  };

  return (
    <Container className="profile-container">
      <Row className="justify-content-md-center">
        <Col md={8} className="profile-card">
          <h2 className="text-center">Your Profile</h2>

          {error && <Alert variant="danger">{error}</Alert>}
          {success && <Alert variant="success">{success}</Alert>}

          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="formName" className="mb-3">
              <Form.Label>Name</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter your name"
                value={profile.nom}
                onChange={(e) => setProfile({ ...profile, nom: e.target.value })}
                required
              />
            </Form.Group>

            <Form.Group controlId="formEmail" className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                placeholder="Enter your email"
                value={profile.email}
                onChange={(e) => setProfile({ ...profile, email: e.target.value })}
                required
              />
            </Form.Group>

            <Form.Group controlId="formPassword" className="mb-3">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="Enter your password"
                value={profile.motDePasse}
                onChange={(e) => setProfile({ ...profile, motDePasse: e.target.value })}
                required
              />
            </Form.Group>

            <Form.Group controlId="formPhone" className="mb-3">
              <Form.Label>Phone</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter your phone number"
                value={profile.telephone}
                onChange={(e) => setProfile({ ...profile, telephone: e.target.value })}
              />
            </Form.Group>

            <Button variant="primary" type="submit" className="w-100 profile-update-button">
              Update Profile
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
}

export default Profile;

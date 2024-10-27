import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Card, Button, Spinner, Alert } from 'react-bootstrap';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './Abonnement.css'; // Import custom CSS for additional styles

const AbonnementList = () => {
  const [abonnements, setAbonnements] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [subscribedAbonnement, setSubscribedAbonnement] = useState(null);
  const utilisateur = { id: 2 }; // Replace with actual user state if applicable
  const navigate = useNavigate();

  // Fetch all abonnements and user's subscription status
  const fetchAbonnements = async () => {
    setLoading(true);
    try {
      const response = await axios.get('https://localhost:7126/api/Abonnement');
      const userSubscriptionResponse = await axios.get(`https://localhost:7126/api/Abonnement/Utilisateur/${utilisateur.id}`);
      
      setAbonnements(response.data.$values || []);
      setSubscribedAbonnement(userSubscriptionResponse.data || null);
    } catch (error) {
      console.error('Error fetching abonnements:', error);
      setError('Could not fetch abonnements.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAbonnements();
  }, []);

  // Handle subscription logic
  const handleSubscribe = async (abonnementId) => { 
    if (subscribedAbonnement) {
      setError("You already have an active subscription.");
      return;
    }
    
    const abonnement = abonnements.find(a => a.id === abonnementId);
    if (abonnement) {
      setSubscribedAbonnement(abonnement);
      navigate(`/paiement/${abonnementId}`);
    } else {
      setError("Selected abonnement not found.");
    }
  };

  return (
    <Container className="abonnement-list-container">
      <h1 className="my-4 text-center">Available Abonnements</h1>
      {loading ? (
        <Spinner animation="border" variant="primary" className="loading-spinner" />
      ) : error ? (
        <Alert variant="danger" className="text-center">{error}</Alert>
      ) : (
        <>
          <Row className="my-4">
            {abonnements.slice(0, 3).map((abonnement) => ( // Show only the first three abonnements
              <Col md={4} key={abonnement.id} className="mb-4">
                <Card className="abonnement-card">
                  <Card.Body className="text-center">
                    <Card.Title className="abonnement-card-title">
                      {abonnement.type.charAt(0).toUpperCase() + abonnement.type.slice(1)} Plan
                    </Card.Title>
                    <Card.Text className="abonnement-card-text">
                      Price: <strong>${abonnement.prix}</strong>
                    </Card.Text>
                    <Button 
                      variant="outline-primary" 
                      onClick={() => handleSubscribe(abonnement.id)}
                      className="my-3 w-100 abonnement-button"
                    >
                      Subscribe
                    </Button>
                  </Card.Body>
                </Card>
              </Col>
            ))}
          </Row>
          {subscribedAbonnement && (
            <div className="my-4">
              <h2 className="text-center">Your Subscribed Abonnement</h2>
              <Card className="shadow-sm">
                <Card.Body>
                  {subscribedAbonnement.$values && subscribedAbonnement.$values.length > 0 ? (
                    <>
                      <Card.Title>
                        {subscribedAbonnement.$values[0].type.charAt(0).toUpperCase() + subscribedAbonnement.$values[0].type.slice(1)} Plan
                      </Card.Title>
                      <Card.Text>
                        Price: <strong>${subscribedAbonnement.$values[0].prix}</strong><br />
                        Start Date: {new Date(subscribedAbonnement.$values[0].dateDebut).toLocaleDateString()}<br />
                        End Date: {new Date(subscribedAbonnement.$values[0].dateFin).toLocaleDateString()}<br />
                      </Card.Text>
                    </>
                  ) : (
                    <Card.Title>No Subscription Available</Card.Title>
                  )}
                </Card.Body>
              </Card>
            </div>
          )}
        </>
      )}
    </Container>
  );
};

export default AbonnementList;

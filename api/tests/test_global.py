import unittest
from httpx import Client

# 7 test integrations happy path
class TestGlobal(unittest.TestCase):
    
    def setUp(self):
        self.base_url = "http://localhost:3000/api/v1"
        self.admin_token = "a1b2c3d4e5"
        self.reader_token = "f6g7h8i9j0"
        self.headers = {"Content-Type": "application/json", "API_KEY": None}
        self.client = Client()
    

    def test_get_all_as_admin(self):
        self.headers["API_KEY"] = self.admin_token
        endpoint: str = f"{self.base_url}/locations"
        response = Client.get(url=endpoint, headers=self.headers)
        self.assertEqual(True, response.status_code == Client.codes.OK)

    def test_get_single_as_admin(self):
        self.headers["API_KEY"] = self.admin_token
        endpoint: str = f"{self.base_url}/warehouses/1"

        response = Client.get(url=endpoint, headers=self.headers)
        self.assertEqual(True, response.status_code == Client.codes.OK)

    def test_get_single_outsider(self):
        self.headers["API_KEY"] = "invalid-key"
        endpoint: str = f"{self.base_url}/items/1"

        response = Client.get(url=endpoint, headers=self.headers)
        self.assertEqual(True, response.status_code == Client.codes.UNAUTHORIZED)

    def test_get_all_outsider(self):
        self.headers["API_KEY"] = "invalid-key"
        endpoint: str = f"{self.base_url}/shipments/1"

        response = Client.get(url=endpoint, headers=self.headers)
        self.assertEqual(True, response.status_code == Client.codes.UNAUTHORIZED)

    def test_create_as_admin(self):
        self.headers["API_KEY"] = self.admin_token

        body = {
            "id": 1,
            "warehouse_id": 1,
            "code": "unit test",
            "name": "unit test test",
            "created_at": "2024-08-05 16:21:20",
            "updated_at": "2024-08-05 16:21:20",
        }

        response = Client.post(
            url=f"{self.base_url}/locations", headers=self.headers, json=body
        )
        self.assertEqual(True, response.status_code == Client.codes.CREATED)

    def test_update_as_admin(self):
        self.headers["API_KEY"] = self.admin_token

        body = {
            "id": 1,
            "name": "sidi cha",
            "address": "Dahlgata 4",
            "city": "Øysteinnes",
            "zip_code": "8817",
            "country": "Norway",
            "contact_name": "Anette Næss",
            "contact_phone": "65319533",
            "contact_email": "grosaether@example.com",
            "created_at": "2012-12-06 05:59:02",
            "updated_at": "2015-08-18 23:46:10",
        }

        response = Client.put(
            url=f"{self.base_url}/clients/1", headers=self.headers, json=body
        )
        self.assertEqual(True, response.status_code == Client.codes.OK)
        
    def test_delete_as_admin(self):
        self.headers["API_KEY"] = self.admin_token

        response = Client.delete(url=f"{self.base_url}/orders/1", headers=self.headers)
        self.assertEqual(True, response.status_code == Client.codes.OK)


if __name__ == "__main__":
    unittest.main()
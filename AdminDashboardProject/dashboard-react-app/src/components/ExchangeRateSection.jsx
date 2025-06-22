import { useState, useEffect } from "react";
import api from "../api/api";

export default function ExchangeRateSection() {
  const [rate, setRate] = useState("");
  const [newRate, setNewRate] = useState("");

  useEffect(() => {
    async function fetchRate() {
      try {
        const response = await api.get("/rate");
        if (response.data) {
          setRate(response.data.rate);
          setNewRate(response.data.rate);
        }
      } catch {
        alert("Ошибка при загрузке курса");
      }
    }
    fetchRate();
  }, []);

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      const response = await api.post("/rate", {
        rate: parseFloat(newRate),
      });
      setRate(response.data.rate);
      alert("Курс обновлён");
    } catch {
      alert("Ошибка при обновлении курса");
    }
  };

  const handleCancel = () => setNewRate(rate);

  return (
    <div className="mt-8 border-t border-gray-300 pt-6 max-w-md mx-auto bg-white p-6 rounded-lg shadow-lg">
      <p className="text-xl font-semibold text-gray-900 mb-4 text-center">
        Текущий курс: <span className="text-purple-600">{rate}</span>
      </p>
      <form
        onSubmit={handleSave}
        className="flex flex-col sm:flex-row flex-wrap gap-4 items-center"
      >
        <input
          type="number"
          step="0.1"
          value={newRate}
          onChange={(e) => setNewRate(e.target.value)}
          placeholder="Новый курс"
          className="flex-grow min-w-0 border border-gray-300 rounded-md px-4 py-3 text-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
        />
        <div className="flex gap-3 flex-shrink-0">
          <button
            type="submit"
            className="bg-purple-600 text-white px-6 py-3 rounded-md shadow-md hover:bg-purple-700 transition-colors duration-300"
          >
            Сохранить
          </button>
          <button
            type="button"
            onClick={handleCancel}
            className="bg-gray-200 text-gray-800 px-6 py-3 rounded-md shadow-md hover:bg-gray-300 transition-colors duration-300"
          >
            Отмена
          </button>
        </div>
      </form>
    </div>
  );
}

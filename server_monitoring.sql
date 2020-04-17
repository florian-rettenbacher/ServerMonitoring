-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 17. Apr 2020 um 14:52
-- Server-Version: 10.1.38-MariaDB
-- PHP-Version: 7.3.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `server_monitoring`
--
CREATE DATABASE IF NOT EXISTS `server_monitoring` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `server_monitoring`;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `log`
--
USE `server_monitoring`;
CREATE TABLE `log` (
  `log_id` int(32) NOT NULL,
  `value` decimal(7,4) NOT NULL,
  `created_at` date DEFAULT NULL,
  `server_id` int(32) NOT NULL,
  `monitoring_type_id` int(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `monitoring_type`
--
USE `server_monitoring`;
CREATE TABLE `monitoring_type` (
  `monitoring_type_id` int(32) NOT NULL,
  `name` varchar(32) CHARACTER SET latin1 NOT NULL,
  `warning_value` int(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `monitoring_type`
--

INSERT INTO `monitoring_type` (`monitoring_type_id`, `name`, `warning_value`) VALUES
(1, 'ram_usage', 80),
(2, 'cpu_usage', 80),
(3, 'main_drive_usage', 80),
(4, 'second_drive_usage', 80),
(5, 'network_usage', 80);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `server`
--
USE `server_monitoring`;
CREATE TABLE `server` (
  `server_id` int(32) NOT NULL,
  `server_name` varchar(64) CHARACTER SET latin1 NOT NULL,
  `ip_adresse` varchar(64) CHARACTER SET latin1 NOT NULL,
  `location` varchar(64) CHARACTER SET latin1 NOT NULL,
  `username` varchar(255) CHARACTER SET latin1 NOT NULL,
  `password` varchar(255) CHARACTER SET latin1 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `log`
--
ALTER TABLE `log`
  ADD PRIMARY KEY (`log_id`),
  ADD KEY `server_id` (`server_id`),
  ADD KEY `monitoring_type_id` (`monitoring_type_id`);

--
-- Indizes für die Tabelle `monitoring_type`
--
ALTER TABLE `monitoring_type`
  ADD PRIMARY KEY (`monitoring_type_id`);

--
-- Indizes für die Tabelle `server`
--
ALTER TABLE `server`
  ADD PRIMARY KEY (`server_id`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `log`
--
ALTER TABLE `log`
  MODIFY `log_id` int(32) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `monitoring_type`
--
ALTER TABLE `monitoring_type`
  MODIFY `monitoring_type_id` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT für Tabelle `server`
--
ALTER TABLE `server`
  MODIFY `server_id` int(32) NOT NULL AUTO_INCREMENT;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `log`
--
ALTER TABLE `log`
  ADD CONSTRAINT `log_ibfk_1` FOREIGN KEY (`server_id`) REFERENCES `server` (`server_id`),
  ADD CONSTRAINT `log_ibfk_2` FOREIGN KEY (`monitoring_type_id`) REFERENCES `monitoring_type` (`monitoring_type_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
